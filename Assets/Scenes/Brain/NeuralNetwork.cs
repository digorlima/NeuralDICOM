using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using TMPro;
using System;
using System.Drawing;
using System.IO;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Data.Image;
using Encog.ML.Train.Strategy;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Util.DownSample;
using Encog.Util.Simple;

public class NeuralNetwork : MonoBehaviour
{
    BasicNetwork basicNetwork;
    IMLDataSet dataPairs;
    IMLTrain backpropagation;
    
    private int epoch = 0;
    double[][] x;
    double[][] y;
    bool train = false;

    [Header("Objects")]
    public GameObject images;

    [Header("Typology")] 
    public int inputLayer = 2;
    public int[] hiddenLayer;
    public int outputLayer = 1;
    public float minimumError;
    public float maximumEpoch;

    public void CreateNetwork()
    {
        double[][] x =
        {
            new[]{0.0, 0.0},
            new[]{0.0, 1.0},
            new[]{1.0, 0.0},
            new[]{1.0, 1.0}
        };

        double[][] y =
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

        backpropagation = new ResilientPropagation(basicNetwork, dataPairs, 0.4, 0.12);

        train = true;
        
        FeedImages();
    }

    void Update()
    {
        if (train){
            Train();
        }
    }

    void Train() {
        backpropagation.Iteration();
        Debug.Log("Epoch: " + epoch + "\nErro: " + backpropagation.Error);
        epoch++;

        if(backpropagation.Error < minimumError || epoch > maximumEpoch)
        {
            backpropagation.FinishTraining();
            train = false;
        }
    }

    public void GetResults(int value)
    {
        var pair = dataPairs[value];

        IMLData output = basicNetwork.Compute(pair.Input);
        Debug.Log(pair.Input[0] + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
    }

    public void FeedImages(){
        foreach (Transform child in images.transform)
        {
            
        }
    }
}
