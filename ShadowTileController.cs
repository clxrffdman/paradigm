using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadowTileController : MonoBehaviour
{
    public GameObject lighter;
    public bool isTriggered;
    public float currentIntensity;
    public float maxIntensity;
    public float targetIntensity;
    public bool active;
    public string[] roomNames;
    public Tilemap tilemap;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("lightPrime");
        isTriggered = false;
        
        currentIntensity = 1f;

        for (int i = 0; i < roomNames.Length; i++)
        {
            tilemap = GameObject.Find(roomNames[i]).GetComponent<Tilemap>();
            tilemap.color = new Color(1.0f, 1.0f, 1.0f, currentIntensity);
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(currentIntensity - targetIntensity) > 0.01f)
        {
            currentIntensity = Mathf.Lerp(currentIntensity,targetIntensity,0.05f);
            for(int i = 0; i < roomNames.Length; i++)
            {
                tilemap = GameObject.Find(roomNames[i]).GetComponent<Tilemap>();
                tilemap.color = new Color(1.0f, 1.0f, 1.0f, currentIntensity);
            }
            


        }
        else
        {
            tilemap.color = new Color(1.0f, 1.0f, 1.0f, currentIntensity);
        }
    }



    void OnTriggerStay2D(Collider2D col)
    {
        active = true;

        if (col.gameObject.tag == ("Player"))
        {
            targetIntensity = 0f;
            isTriggered = true;
            player.GetComponent<PlayerLight>().playerLightEnable = false;



        }

    }

    void OnTriggerExit2D(Collider2D col)
    {

        active = false;
        if (col.gameObject.tag == ("Player"))
        {
            isTriggered = false;
            targetIntensity = maxIntensity;
            player.GetComponent<PlayerLight>().playerLightEnable = true;

        }

    }
}
