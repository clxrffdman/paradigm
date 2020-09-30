using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBox : MonoBehaviour
{

    public bool active;
    public Component[] objs;

    void Start()
    {
        objs = GetComponentsInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            LeanTween.value(gameObject, 1f, 0.1f, 0.2f).setOnUpdate((float val) =>
            {
                foreach(SpriteRenderer obj in objs)
                {
                    obj.color = new Color(1, 1, 1, val);

                }
                


            });
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            LeanTween.value(gameObject, 0.1f, 1f, 0.2f).setOnUpdate((float val) =>
            {

                foreach (SpriteRenderer obj in objs)
                {
                    obj.color = new Color(1, 1, 1, val);

                }


            });
        }
    }


    void DisableChildren()
    {

    }
}
