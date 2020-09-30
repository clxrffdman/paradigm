using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroManager : MonoBehaviour
{

    public bool norm;
    // Start is called before the first frame update
    void Start()
    {
        norm = true;
    }

    // Update is called once per frame
    void Update()
    {





        /*if(norm == true)
        {
            GetComponent<CameraMouse2>().enabled = true;
            GetComponent<CameraEnviro>().enabled = false;
        }
        if (norm == false)
        {
                GetComponent<CameraMouse2>().enabled = false;
                GetComponent<CameraEnviro>().enabled = true;
            
        }
        */
        if (norm == true)
        {
            GetComponent<CameraMouse2>().enabled = true;
            
            GetComponent<CameraEnviro>().enabled = false;
        }
        if (norm == false)
        {
            GetComponent<CameraMouse2>().enabled = false;
            GetComponent<CameraEnviro>().enabled = true;
        }
        

    }

    void DeactivateLerp()
    {
        GetComponent<CameraMouse2>().isLerp = false;
    }

    void ActivateLerp()
    {
        GetComponent<CameraMouse2>().isLerp = true;
    }


}
