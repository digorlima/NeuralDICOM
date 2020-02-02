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

    public int actual = (int)Types.Normal;

    public TextMeshProUGUI label;

    public void Pass()
    {
        if (actual == 2)
        {
            actual = 0;
        }
        else
        {
            actual++;
        }

        switch (actual)
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

