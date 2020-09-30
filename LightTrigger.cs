using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Experimental.Rendering;

public class LightTrigger : MonoBehaviour
{

    public GameObject lighter;
    public bool isTriggered;
    public float currentIntensity;
    public float maxIntensity;
    public float targetIntensity;
    public bool active;
    public Light2D light2d;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("lightPrime");
        isTriggered = false;
        
        //lighter = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(light2d.intensity - targetIntensity) > 0.00001f)
        {

            int children = transform.childCount;
            
               light2d.intensity = Mathf.Lerp(light2d.intensity, targetIntensity, 0.05f);
            
            

        }
    }

    

    void OnTriggerStay2D(Collider2D col)
    {
        /*
        active = true;
        
        if (col.gameObject.tag == ("Player"))
        {
            targetIntensity = maxIntensity;
            isTriggered = true;
            player.GetComponent<PlayerLight>().playerLightEnable = false;



        }
        */
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        /*
        active = false;
        if (col.gameObject.tag == ("Player"))
        {
            isTriggered = false;
            targetIntensity = 0f;
            player.GetComponent<PlayerLight>().playerLightEnable = true;

        }
        */
       
    }

    public void MakeBright()
    {
        active = true;

       
        targetIntensity = maxIntensity;
        isTriggered = true;
        //player.GetComponent<PlayerLight>().playerLightEnable = false;
        if(GetComponent<BoxCollider2D>() != null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if (GetComponent<PolygonCollider2D>() != null)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }




    }
}
