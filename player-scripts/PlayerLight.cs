using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public bool playerLightEnable;

    public float currentIntensity;
    public float maxIntensity;
    public float targetIntensity;




    // Start is called before the first frame update
    void Start()
    {
        playerLightEnable = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerLightEnable)
        {
            targetIntensity = maxIntensity;
        }

        if (!playerLightEnable)
        {
            targetIntensity = 0f;
        }

        if (Mathf.Abs(GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity-targetIntensity)>0.0001)
        {
            GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = Mathf.Lerp(GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity, targetIntensity, 0.1f);
        }
        


        
    }
}
