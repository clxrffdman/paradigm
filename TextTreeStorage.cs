using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTreeStorage : MonoBehaviour
{
    public string TextChoiceArray(int index)
    {
        int caseSwitch = index;
        string rv = "";
        switch (caseSwitch)
        {
            case 1:
                rv = "Description Pending";
                break;
            case 2:
                rv = "Description Pending";
                break;
            case 3:
                rv = "Description Pending";
                break;
            case 4:
                rv = "Description Pending";
                break;
            case 5:
                rv = "A bolt-action rifle with some mysterious cloth on the barrel.";
                break;
            case 6:
                rv = "An old fashioned revolver found in a ditch somewhere. 'Made by Walmart'";
                break;
            case 7:
                rv = "Description Pending";
                break;
            default:
                rv = "UNARMED";
                break;
        }

        return rv;
    }
}
