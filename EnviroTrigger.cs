using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroTrigger : MonoBehaviour
{

    public GameObject cam;
    public bool isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        isTriggered = false;
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        cam.GetComponent<CameraEnviro>().Enviro = transform.GetChild(0);

        if (col.gameObject.tag == ("Player"))
        {
            isTriggered = true;
            cam.GetComponent<EnviroManager>().norm = false;

        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == ("Player"))
        {
            isTriggered = false;
            cam.GetComponent<EnviroManager>().norm = true;

        }
       
    }
}
