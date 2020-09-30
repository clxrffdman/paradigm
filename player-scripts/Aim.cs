using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{

    public Transform target;
    public SpriteRenderer sprite;
    public float rotation;
    public bool top;
    public bool right;

    public bool isFlipped;

    public float addAngle;

    public bool unarmed;

    public proxyCursor proxycursor;

    public bool aiming;

    public Shoot rootShoot;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        rootShoot = GameObject.Find("RootShoot").transform.GetComponent<Shoot>();

        if(proxycursor == null)
        {
            if(GameObject.Find("CrosshairProxy") != null)
            {
                proxycursor = GameObject.Find("CrosshairProxy").GetComponent<proxyCursor>();
            }
        }
    }

    void FixedUpdate()
    {
        //gun flip
        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
        {
            right = false;
            if (!isFlipped)
            {
                sprite.flipY = true;
            }
            else
            {
                sprite.flipY = false;
            }


            transform.GetChild(0).transform.localPosition = new Vector3(transform.GetChild(0).transform.localPosition.x, -Mathf.Abs(transform.GetChild(0).transform.localPosition.y), transform.GetChild(0).transform.localPosition.z);
        }
        else
        {
            right = true;
            if (!isFlipped)
            {
                sprite.flipY = false;
            }
            else
            {
                sprite.flipY = true;
            }

            transform.GetChild(0).transform.localPosition = new Vector3(transform.GetChild(0).transform.localPosition.x, Mathf.Abs(transform.GetChild(0).transform.localPosition.y), transform.GetChild(0).transform.localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (aiming)
        {
            transform.right = target.position - transform.position;
            transform.Rotate(new Vector3(0, 0, addAngle));
            rotation = transform.rotation.eulerAngles.z;

            if (!unarmed && ((proxycursor.proxyAngleOfShot > 0 && proxycursor.proxyAngleOfShot < 180) || (proxycursor.proxyAngleOfShot > 360 && proxycursor.proxyAngleOfShot < 540)))
            {
                top = true;
                sprite.sortingOrder = -1;

            }
            else
            {
                top = false;
                sprite.sortingOrder = 1;
            }

            if (rootShoot.currentGunIndex == 7 && gameObject.transform.parent.gameObject.GetComponent<PlayerAnim>().run && (Input.GetKey("a") || Input.GetKey("d")))
            {
                top = true;
                sprite.sortingOrder = -1;
            }
            else
            {
                if (gameObject.transform.parent.gameObject.GetComponent<PlayerAnim>().run)
                {

                    if (!Input.GetKey("s") && !Input.GetKey("a") && !Input.GetKey("d"))
                    {
                        top = true;
                        sprite.sortingOrder = -1;
                    }

                    if (Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("d") && transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < 360)
                    {
                        top = true;
                        sprite.sortingOrder = 1;
                    }

                    if ((Input.GetKey("a") || Input.GetKey("d")) && (proxycursor.proxyAngleOfShot > 180 && proxycursor.proxyAngleOfShot < 360))
                    {
                        top = true;
                        sprite.sortingOrder = -1;
                    }

                }
                

                
            }





            
        }





    }
    
}
