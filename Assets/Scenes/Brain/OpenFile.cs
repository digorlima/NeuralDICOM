using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Image = UnityEngine.UI.Image;
using OpenCvSharp;
using UnityScript.Steps;
using Rect = UnityEngine.Rect;

public class OpenFile : MonoBehaviour
{
    public GameObject grid;
    public GameObject preset;

    public float highOffset;
    public float lowOffset;
    
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
        //        Mat mat = new Mat(texture.height, texture.width, MatType.CV_8UC3, Scalar.Black);
//        mat = Cv2.ImRead("C:/Users/danie/Documents/GitHub/NeuralDICOM/Assets/Scenes/Brain/lena.pgm");
//                                       
//                                               int erosion_size = 1;
//                                               
//                                               Mat element = Cv2.GetStructuringElement( 0,
//                                                   new Size( 2*erosion_size + 1, 2*erosion_size+1 ),
//                                                   new Point( erosion_size, erosion_size ) );
//                                               
//                                               Cv2.Erode(mat, mat, element);
//        
//        mat = TextureToMat(texture, mat);
//        
//        Cv2.ImShow("teste", mat);
        texture = removeUselessPixels(texture, 0);
        texture = removeBlacks(texture);

        return texture;
    }

    public Texture2D removeUselessPixels(Texture2D texture, int offset)
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

    public Texture2D removeBlacks(Texture2D texture)
    {
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

                if (average > lowOffset && average < highOffset)
                {
                    break;
                }
                else
                {
                    texture.SetPixel(column, row, new Color(0,0,0,0));
                }
            }
        }

        for (int row = 0; row < texture.height; row++)
        {
            for (int column = texture.width; column > 0; column--)
            {
                float average = 0;

                for (int channel = 0; channel < 3; channel++)
                {
                    average += texture.GetPixel(column, row)[channel];
                }

                average /= 3.0f;

                if (average > lowOffset && average < highOffset)
                {
                    break;
                }
                else
                {
                    texture.SetPixel(column, row, new Color(0,0,0,0));
                }
            }
        }

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

                if (average > lowOffset && average < highOffset)
                {
                    break;
                }
                else
                {
                    texture.SetPixel(column, row, new Color(0,0,0,0));
                }
            }
        }

        for (int column = 0; column < texture.width; column++)
        {
            for (int row = texture.height; row > 0; row--)
            {
                float average = 0;

                for (int channel = 0; channel < 3; channel++)
                {
                    average += texture.GetPixel(column, row)[channel];
                }

                average /= 3.0f;

                if (average > lowOffset && average < highOffset)
                {
                    break;
                }
                else
                {
                    texture.SetPixel(column, row, new Color(0,0,0,0));
                }
            }
        }

        return texture;
    }
    
    /*unsafe public Mat TextureToMat(Texture2D texture, Mat mat)
    {
        for (int row = 0; row < mat.Rows; row++)
        {
            for (int column = 0; column < mat.Cols; column++)
            {
                for (int channels = 0; channels < mat.Channels(); channels++)
                {
                    var ptr = mat.Ptr(row, column) + channels;
                    ptr = (IntPtr) 35;
                }
            }
        }

        return mat;
    }*/
}
