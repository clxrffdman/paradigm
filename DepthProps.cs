using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthProps : MonoBehaviour
{
    public string initialLayer;
    public int initialOrder;

    public bool depthChanging = true;

    public string finalLayer;
    public int finalOrder;

    public SpriteRenderer spriteRenderer;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) {
            player = GameObject.Find("Player");
        }

        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        initialOrder = spriteRenderer.sortingOrder;
        finalOrder = spriteRenderer.sortingOrder;



        StopAllCoroutines();
        StartCoroutine(depthChange());
    }

    // Update is called once per frame
    

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(depthChange());
    }

    public IEnumerator depthChange()
    {

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        while (depthChanging)
        {
            if(transform.position.y < player.transform.position.y)
            {
                spriteRenderer.sortingLayerName = finalLayer;
                spriteRenderer.sortingOrder = finalOrder;
            }
            else
            {
                spriteRenderer.sortingLayerName = initialLayer;
                spriteRenderer.sortingOrder = initialOrder;
            }




            yield return new WaitForSeconds(0.25f);
        }

    }
}
