using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTriggerDirection : MonoBehaviour
{

    public bool isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        isTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            isTriggered = true;


        }

    }

    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == ("Player"))
        {
            isTriggered = false;


        }

    }
    

}
