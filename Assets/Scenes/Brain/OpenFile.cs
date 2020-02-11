using System;
using System.Drawing;
using System.IO;
using Boo.Lang;
using UnityEngine;
using UnityEditor;
using Image = UnityEngine.UI.Image;
using OpenCvSharp;
using SFB;
using UnityScript.Steps;
using Rect = UnityEngine.Rect;

public class OpenFile : MonoBehaviour
{
    public GameObject grid;
    public GameObject preset;

//    public float highOffset;
//    public float lowOffset;

    public static List<Texture2D> Open() {
        //string path = EditorUtility.OpenFilePanel("Open a image", "", "png;*jpg;*jpeg;*");
        List<Texture2D> textures = new List<Texture2D>();
        
        ExtensionFilter[] extensions = new [] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
        };
        
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open a image", "", extensions, true);

        foreach(string path in paths)
        {
            Texture2D texture;
        
            var fileContent = File.ReadAllBytes(path);

            texture = new Texture2D(0, 0);
            texture.LoadImage(fileContent);

            texture = Clean(texture);
            TextureScaler.scale(texture, 100, 100, 0);
            texture.Apply();
            
            textures.Add(texture);
        }

        return textures;
    }

    public void PutInObject()
    {
        foreach (Texture2D texture in Open())
        {
            GameObject image = Instantiate(preset, grid.transform, false);
            image.transform.GetChild(1).GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    public static Texture2D Clean(Texture2D texture)
    {
        texture = RemoveUselessPixels(texture, 0);
        texture = Limiarize(texture);
        //texture = removeBlacks(texture);

        return texture;
    }

    public static Texture2D RemoveUselessPixels(Texture2D texture, int offset)
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
        
        for (int column = 0; column < texture.width; column++)
        {
            for (int row = 0; row < texture.height; row++)
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
        
        for (int column = texture.width; column > 0; column--)
        {
            for (int row = texture.height; row > 0; row--)
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
        
        Texture2D newTexture = new Texture2D(endColumn - (startColumn + offset*2),endRow - (startRow + offset*2));
        
        for (int row = 0; row < newTexture.height; row++)
        {
            for (int column = 0; column < newTexture.width; column++)
            {
                newTexture.SetPixel(column, row, texture.GetPixel(column + startColumn + offset, row + startRow + offset));
            }
        }

        return newTexture;
    }

    public static Texture2D Limiarize(Texture2D texture)
    {
        for (int row = 0; row < texture.height; row++)
        {
            for (int columns = 0; columns < texture.width; columns++)
            {
                if ((texture.GetPixel(columns, row)[0]) < 0.2f)
                {
                    texture.SetPixel(columns, row, new Color(0, 0, 0));
                }
                else if (texture.GetPixel(columns, row)[0] < 0.6f &&
                         texture.GetPixel(columns, row)[0] > 0.2f)
                {
                    texture.SetPixel(columns, row, new Color(0.5f, 0.5f, 0.5f));
                }
                else
                {
                    texture.SetPixel(columns, row, new Color(1, 1, 1));
                }
            }
        }

        return texture;
    }
    
//    public Texture2D removeBlacks(Texture2D texture)
//    {
//        for (int row = 0; row < texture.height; row++)
//        {
//            for (int column = 0; column < texture.width; column++)
//            {
//                float average = 0;
//
//                for (int channel = 0; channel < 3; channel++)
//                {
//                    average += texture.GetPixel(column, row)[channel];
//                }
//
//                average /= 3.0f;
//
//                if (average > lowOffset && average < highOffset)
//                {
//                    break;
//                }
//                else
//                {
//                    texture.SetPixel(column, row, new Color(0,0,0,0));
//                }
//            }
//        }
//
//        for (int row = 0; row < texture.height; row++)
//        {
//            for (int column = texture.width; column > 0; column--)
//            {
//                float average = 0;
//
//                for (int channel = 0; channel < 3; channel++)
//                {
//                    average += texture.GetPixel(column, row)[channel];
//                }
//
//                average /= 3.0f;
//
//                if (average > lowOffset && average < highOffset)
//                {
//                    break;
//                }
//                else
//                {
//                    texture.SetPixel(column, row, new Color(0,0,0,0));
//                }
//            }
//        }
//
//        for (int column = 0; column < texture.width; column++)
//        {
//            for (int row = 0; row < texture.height; row++)
//            {
//                float average = 0;
//
//                for (int channel = 0; channel < 3; channel++)
//                {
//                    average += texture.GetPixel(column, row)[channel];
//                }
//
//                average /= 3.0f;
//
//                if (average > lowOffset && average < highOffset)
//                {
//                    break;
//                }
//                else
//                {
//                    texture.SetPixel(column, row, new Color(0,0,0,0));
//                }
//            }
//        }
//
//        for (int column = 0; column < texture.width; column++)
//        {
//            for (int row = texture.height; row > 0; row--)
//            {
//                float average = 0;
//
//                for (int channel = 0; channel < 3; channel++)
//                {
//                    average += texture.GetPixel(column, row)[channel];
//                }
//
//                average /= 3.0f;
//
//                if (average > lowOffset && average < highOffset)
//                {
//                    break;
//                }
//                else
//                {
//                    texture.SetPixel(column, row, new Color(0,0,0,0));
//                }
//            }
//        }
//
//        return texture;
//    }
}
