using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.ML.Train.Strategy;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Networks.Training.Propagation.Manhattan;
using Encog.Neural.Networks.Training.Propagation.Quick;
using Encog.Neural.Networks.Training.Strategy;
using Encog.Util.Simple;
using TMPro;
using UnityEngine.UI;

public class NeuralNetwork : MonoBehaviour
{
    BasicNetwork basicNetwork;
    IMLDataSet dataPairs;
    IMLTrain backpropagation;
    
    private int epoch = 0;
    private double[][] x;
    private double[][] y;

    [Header("Objects")]
    public GameObject images;
    public Button testButton;
    public TextMeshProUGUI debugText;
    public TMP_InputField minimumError;
    public TMP_InputField maximumEpoch;

    [Header("Typology")] 
    public int inputLayer;
    public int[] hiddenLayer;
    public int outputLayer;
//    public float learningRate;
//    public float momentum;
    
    private IEnumerator coroutine;

    public void CreateNetwork()
    {
        FeedImages();

        double[][] a =
        {
            new[]{0.0, 0.0},
            new[]{0.0, 1.0},
            new[]{1.0, 0.0},
            new[]{1.0, 1.0}
        };

        double[][] b =
        {
            new[]{0.0},
            new[]{1.0},
            new[]{1.0},
            new[]{0.0}
        };

        basicNetwork = new BasicNetwork();
        basicNetwork.AddLayer(new BasicLayer(inputLayer));
        for (int i = 0; i < hiddenLayer.Length; i++)
        {
            basicNetwork.AddLayer(new BasicLayer(hiddenLayer[i]));
        }
        basicNetwork.AddLayer(new BasicLayer(outputLayer));
        basicNetwork.Structure.FinalizeStructure();
        basicNetwork.Reset();

        dataPairs = new BasicMLDataSet(x, y);
        
        backpropagation = new ResilientPropagation(basicNetwork, dataPairs);
        //backpropagation = new Backpropagation(basicNetwork, dataPairs, learningRate, momentum);
        //backpropagation.AddStrategy(new ResetStrategy(0.5, 50));
        
        epoch = 0;

        maximumEpoch.interactable = false;
        minimumError.interactable = false;

        if (maximumEpoch.text == "")
        {
            maximumEpoch.text = "1000";
        }

        if (minimumError.text == "")
        {
            minimumError.text = "0.01";
        }

        coroutine = Train();
        StartCoroutine(coroutine);
    }

    public void Stop()
    {
        StopCoroutine(coroutine);
        testButton.interactable = true;
        debugText.text = "Done";
        maximumEpoch.interactable = true;
        minimumError.interactable = true;
    }
    
    IEnumerator Train() {
        while (true)
        {
            backpropagation.Iteration();
            debugText.text = ("Epoch: " + epoch + "\nErro: " + backpropagation.Error);
            epoch++;

            if (epoch > int.Parse(maximumEpoch.text) || backpropagation.Error < float.Parse(minimumError.text, CultureInfo.InvariantCulture))
            {
                Stop();
            }
            
            yield return null;
        }
    }

    public void GetResults()
    {
        foreach (Texture2D texture in OpenFile.Open())
        {
            IMLDataPair pair = new BasicMLDataPair(new BasicMLData(Texture2List(texture).ToArray()));

            IMLData output = basicNetwork.Compute(pair.Input);

            string result = "";
            float ideal = 0;
        
            if (output[0] < 0.25f)
            {
                result = "Normal";
            }
            else if (output[0] > 0.25f && output[0] < 0.75f)
            {
                result = "Esquemico";
                ideal = 0.5f;
            }
            else
            {
                result = "Hemorragico";
                ideal = 1.0f;
            }
        
            Debug.Log("Isso parece ser: " + result + "\nEquivalência{ Imagem Testada: " + output[0] + " Imagem Original: " + ideal + " }");
            debugText.text = ("Isso parece ser: " + result + "\nEquivalência{ Imagem Testada: " + output[0] + " Imagem Original: " + ideal + " }");
        }
    }

    public void FeedImages()
    {
        List<double> imageList = new List<double>();
        List<double> typeList = new List<double>();
        
        int i = 0;

        foreach (Transform child in images.transform)
        {
            i++;
        }

        x = new double[i][];
        y = new double[i][];

        i = 0;

        foreach (Transform child in images.transform)
        {
            Texture2D texture = child.GetChild(1).GetComponent<Image>().sprite.texture;
            BrainType type = child.gameObject.GetComponent<BrainType>();
            
            imageList = Texture2List(texture);
            typeList.Add(type.identity);

            x[i] = imageList.ToArray();
            y[i] = typeList.ToArray();

            i++;
            imageList.Clear();
            typeList.Clear();
        }
    }

    public List<double> Texture2List(Texture2D texture)
    {
        List<double> averagesRows = new List<double>();
        
        double sum = 0.0;
        int i = 0;
        for (int row = 0; row < texture.height; row++)
        {
            for (int columns = 0; columns < texture.width; columns++)
            {
                sum += texture.GetPixel(columns, row)[0];
                i++;
            }
            averagesRows.Add(sum / i);
        }

        return averagesRows;
    }
}
