using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameControl : MonoBehaviour
{
    // Start is called before the first frame update

    void Awake()
    {
        
        


    }
    void Start()
    {
        Invoke("Frames", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Frames()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;// VSync must be disabled
    }
}
