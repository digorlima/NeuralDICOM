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

public class EncogTest : MonoBehaviour
{
    BasicNetwork basicNetwork;
    IMLDataSet dataPairs;
    IMLTrain backpropagation;
    private int epoch = 0;

    double[][] x;
    double[][] y;

    bool train = true;

    public GameObject buttons;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI idealText;

    void Start()
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
        basicNetwork.AddLayer(new BasicLayer(2));
        basicNetwork.AddLayer(new BasicLayer(4));
        basicNetwork.AddLayer(new BasicLayer(1));
        basicNetwork.Structure.FinalizeStructure();
        basicNetwork.Reset();

        dataPairs = new BasicMLDataSet(x, y);

        backpropagation = new ResilientPropagation(basicNetwork, dataPairs, 0.4, 0.12);

        
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

        if(backpropagation.Error < 0.001 || epoch > 3500)
        {
            backpropagation.FinishTraining();
            train = false;
            buttons.SetActive(true);
        }

        
    }

    public void GetResults(int value)
    {
        resultText.gameObject.SetActive(true);
        idealText.gameObject.SetActive(true);

        var pair = dataPairs[value];

        IMLData output = basicNetwork.Compute(pair.Input);
        Debug.Log(pair.Input[0] + @"," + pair.Input[1] + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
        resultText.text = "Result: " + output[0];
        idealText.text = "Ideal: " + pair.Ideal[0];
    }
}
