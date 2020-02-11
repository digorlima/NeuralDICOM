using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BrainType : MonoBehaviour
{
    public enum Types{Normal, Esquemico, Hemorragico};
    
    public float identity = 0;
    public Texture2D texture;
    public TextMeshProUGUI label;

    private void Start()
    {
        texture = gameObject.transform.GetChild(1).GetComponent<Image>().sprite.texture;
    }

    public void Pass()
    {
        if (identity == 1)
        {
            identity = 0;
        }
        else
        {
            identity += 0.5f;
        }

        switch (identity)
        {
            case 0:
                label.text = "Normal";
                break;
            
            case 0.5f:
                label.text = "Esquemico";
                break;
            
            case 1:
                label.text = "Hemorragico";
                break;
        }
    }
}

