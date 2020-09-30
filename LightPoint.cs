using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoint : MonoBehaviour
{
    public GameObject player;
    public GameObject lighter;
    public float dist;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        lighter = gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //cam.GetComponent<EnviroManager>().norm = true;
        /*
        dist = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y));
        if(dist <= 3)
        {
            cam.GetComponent<EnviroManager>().norm = false;
            cam.GetComponent<CameraEnviro>().Enviro = transform;
            
        }

        */
        /*else if(cam.GetComponent<EnviroManager>().norm == false)
        {
            cam.GetComponent<EnviroManager>().norm = true;
        }
        */
    }

    void Update()
    {
        
    }
}
