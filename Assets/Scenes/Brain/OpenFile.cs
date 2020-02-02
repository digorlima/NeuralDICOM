using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Doozy.Engine.UI;
using Doozy.Engine.Utils.ColorModels;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
public class OpenFile : MonoBehaviour
{
    public GameObject grid;
    public GameObject preset;
    
    public void Open() {
        string path = EditorUtility.OpenFilePanel("Open a image", "", "png;*jpg;*jpeg;*");
        string extension = "";

        Texture2D tex;
        
        var fileContent = File.ReadAllBytes(path);

        tex = new Texture2D(0, 0);
        tex.LoadImage(fileContent);

        tex = Clean(tex);
        tex.Apply();

        GameObject image = Instantiate(preset, grid.transform, false);
        image.transform.GetChild(1).GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public Texture2D Clean(Texture2D texture)
    {
        int startRow = 0;
        int startColumn = 0;
        
        int endRow = 0;
        int endColumn = 0;
        
        bool brk = false;

        for (int row = 0; row < texture.height; row++)
        {
            for (int column = 0; column < texture.width; column++)
            {
                float average = 0;

                for (int channel = 0; channel < 3; channel++)
                {
                    average += texture.GetPixel(column, row)[channel];
                }

                average /= 3.0f;

                if (average > 1.0f/3.0f)
                {
                    startRow = row;
                    brk = true;
                    break;
                }
            }

            if (brk)
            {
                break;
            }
        }

        brk = false;
        
        for (int row = texture.height; row > 0; row--)
        {
            for (int column = texture.width; column > 0; column--)
            {
                float average = 0;

                for (int channel = 0; channel < 3; channel++)
                {
                    average += texture.GetPixel(column, row)[channel];
                }

                average /= 3.0f;

                if (average > 1.0f/3.0f)
                {
                    endRow = row;
                    brk = true;
                    break;
                }
            }

            if (brk)
            {
                break;
            }
        }

        brk = false;
        
        for (int column = 0; column < texture.height; column++)
        {
            for (int row = 0; row < texture.width; row++)
            {
                float average = 0;

                for (int channel = 0; channel < 3; channel++)
                {
                    average += texture.GetPixel(column, row)[channel];
                }

                average /= 3.0f;

                if (average > 1.0f/3.0f)
                {
                    startColumn = column;
                    brk = true;
                    break;
                }
            }

            if (brk)
            {
                break;
            }
        }
        
        brk = false;
        
        for (int column = texture.height - 1; column > 0; column--)
        {
            for (int row = texture.width; row > 0; row--)
            {
                float average = 0;

                for (int channel = 0; channel < 3; channel++)
                {
                    average += texture.GetPixel(column, row)[channel];
                }

                average /= 3.0f;

                if (average > 1.0f/3.0f)
                {
                    endColumn = column;
                    brk = true;
                    break;
                }
            }

            if (brk)
            {
                break;
            }
        }
        
        Texture2D newTexture = new Texture2D(texture.width - startColumn - (texture.width - endColumn), texture.height - startRow - (texture.height - endRow));
        
        for (int row = 0; row < newTexture.height; row++)
        {
            for (int column = 0; column < newTexture.width; column++)
            {
                newTexture.SetPixel(column, row, texture.GetPixel(column + startColumn, row + startRow));
            }
        }

        return newTexture;
    }
}
