using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSwapTrigger : MonoBehaviour
{
    
    public GameObject inside;
    public GameObject outside;
    public GameObject barrierIn;
    public GameObject barrierOut;
    public bool isTriggered;
    public bool off;

    public bool oneWay;
    public bool IN;
    // Start is called before the first frame update
    void Start()
    {
        
        
        isTriggered = false;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        
        if (col.gameObject.tag == ("Player") && off == false)
        {
            isTriggered = true;

            if (!oneWay)
            {
                
                if (GameObject.Find("Player").GetComponent<PlayerBehaviour>().outCave == true)
                {

                    col.gameObject.GetComponent<PlayerBehaviour>().ChangeTile(false);
                    barrierOut.SetActive(false);
                    barrierIn.SetActive(true);
                    LeanTween.move(col.gameObject, inside.transform.position, 0.18f);

                }
                else if (GameObject.Find("Player").GetComponent<PlayerBehaviour>().outCave == false)
                {
                    col.gameObject.GetComponent<PlayerBehaviour>().ChangeTile(true);
                    barrierIn.SetActive(false);
                    barrierOut.SetActive(true);
                    LeanTween.move(col.gameObject, outside.transform.position, 0.18f);

                }
            }

            if (oneWay)
            {
                if (GameObject.Find("Player").GetComponent<PlayerBehaviour>().outCave == true && IN)
                {

                    col.gameObject.GetComponent<PlayerBehaviour>().ChangeTile(false);
                    LeanTween.move(col.gameObject, inside.transform.position, 0.18f);

                }
                else if (GameObject.Find("Player").GetComponent<PlayerBehaviour>().outCave == false && !IN)
                {
                    col.gameObject.GetComponent<PlayerBehaviour>().ChangeTile(true);

                    LeanTween.move(col.gameObject, outside.transform.position, 0.18f);

                }
            }
            


            

            
            
            
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
