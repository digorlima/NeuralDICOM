using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrainType : MonoBehaviour
{
    enum Types{Normal, Hemorragia, Coagulo};
    public int identity = (int)Types.Normal;
    public TextMeshProUGUI label;

    public void Pass()
    {
        if (identity == 2)
        {
            identity = 0;
        }
        else
        {
            identity++;
        }

        switch (identity)
        {
            case (int)Types.Normal:
                label.text = "Normal";
                break;
            
            case (int)Types.Hemorragia:
                label.text = "Hemorragia";
                break;
            
            case (int)Types.Coagulo:
                label.text = "Coagulo";
                break;
        }
    }
}

