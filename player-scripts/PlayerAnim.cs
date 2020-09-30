using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{

    public Transform target;
    public SpriteRenderer sprite;
    public float rotation;
    public bool left;
    public bool up;

    GameObject levelControl;

    public bool run;
    public float animNorm;
    public float animSprint;
    public bool animCheck;

    public bool mUp;
    public bool mLeft;

    public Animator anim;

    

    // Start is called before the first frame update
    void Start()
    {
        levelControl = GameObject.Find("LevelController");
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        if (run == true)
        {

        }

        if (run == false)
        {

        }
    }

    public void ReturnToIdle()
    {
        animCheck = false;
        run = false;
        anim.SetBool("isRun", false);
    }

    public void ReturnToChecking()
    {
        animCheck = true;
    }

    void LateUpdate()
    {
        if (levelControl.GetComponent<PauseMenu>().paused == false && levelControl.GetComponent<PauseMenu>().inInventory == false && animCheck == true)
        {
            rotation = transform.position.x;
            if (transform.position.x > target.transform.position.x)
            {
                left = true;
                anim.SetBool("isLeft", true);

            }
            else
            {
                left = false;
                anim.SetBool("isLeft", false);
            }

            if (transform.position.y > target.transform.position.y)
            {
                up = false;
                anim.SetBool("isUp", false);

            }
            else
            {
                up = true;
                anim.SetBool("isUp", true);
            }

            ///////////////




            if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
            {

                run = true;
                anim.SetBool("isRun", true);
            }
            else
            {

                run = false;
                anim.SetBool("isRun", false);

            }




            CheckMoveDirection();

            /*if (up && !mUp)
            {
                anim.SetBool("w", true);
                anim.SetBool("s", false);
            }

            if (!up && mUp)
            {
                anim.SetBool("s", true);
                anim.SetBool("w", false);
            }
            */


            CheckSprint();



            //ANIM WHILE SHOOTING BELOW
            /*
            if (Input.GetKey(KeyCode.Mouse0))
            {
                anim.SetBool("isRun", false);
            }
            */

        }

        

    }

    void clearDir()
    {
        anim.SetBool("w", false);
        anim.SetBool("a", false);
        anim.SetBool("s", false);
        anim.SetBool("d", false);
        mUp = false;
        mLeft = false;
        


    }

    void CheckSprint()
    {
        
        

        if(gameObject.GetComponent<PlayerController>().moveSpeed == gameObject.GetComponent<PlayerController>().moveSpeedSprint && run)
        {
            anim.SetFloat("runMulti", 3);
            
        }
        else if(run)
        {
            anim.SetFloat("runMulti", 2);
            
        }

        //
        
        if(run && !mUp && mLeft && Input.GetKey("a"))
        {
            anim.SetBool("s", false);
            anim.SetBool("a", true);
        }
        if (run && !mUp && !mLeft && Input.GetKey("d"))
        {
            anim.SetBool("s", false);
            anim.SetBool("d", true);
        }
        if(Input.GetKeyUp("d") && Input.GetKey("s"))
        {
            anim.SetBool("s", true);
        }

        if (Input.GetKeyUp("a") && Input.GetKey("s"))
        {
            anim.SetBool("s", true);
        }


        ///CLUTTER BELOW


        if ((Input.GetKey("w") || Input.GetKey("s")) && up)
        {
            anim.SetBool("w", true);
        }

        if ((Input.GetKey("w") || Input.GetKey("s")) && !up)
        {
            anim.SetBool("s", true);
        }




        ////
        ///
        if (run && mUp && mLeft && Input.GetKey("a"))
        {
            anim.SetBool("w", false);
            anim.SetBool("a", true);
        }
        if (run && mUp && !mLeft && Input.GetKey("d"))
        {
            anim.SetBool("w", false);
            anim.SetBool("d", true);
        }

        //

        if (Input.GetKeyUp("a") && Input.GetKey("w"))
        {
            anim.SetBool("w", true);
        }

        if (Input.GetKeyUp("d") && Input.GetKey("w"))
        {
            anim.SetBool("w", true);
        }

    }

    

    void CheckMoveDirection()
    {
        if (Input.GetKeyDown("w"))
        {
            run = true;
            
            anim.SetBool("w", true);
            anim.SetBool("s", false);
            mUp = true;
        }

        if (Input.GetKeyDown("s"))
        {
            run = true;
            
            anim.SetBool("s", true);
            anim.SetBool("w", false);
            mUp = false;
        }

        if (Input.GetKeyUp("s") && Input.GetKey("w"))
        {
            run = true;
            
            anim.SetBool("w", true);
            anim.SetBool("s", false);
            mUp = true;
        }

        if (Input.GetKeyUp("w") && Input.GetKey("s"))
        {
            run = true;
            
            anim.SetBool("s", true);
            anim.SetBool("w", false);
            mUp = false;
        }



        if (Input.GetKeyDown("a"))
        {
            run = true;
            
            anim.SetBool("a", true);
            anim.SetBool("d", false);
            mLeft = true;
        }

        if (Input.GetKeyDown("d"))
        {
            run = true;
            
            anim.SetBool("d", true);
            anim.SetBool("a", false);
            mLeft = false;
        }

        if (Input.GetKeyUp("d") && Input.GetKey("a"))
        {
            run = true;
            
            anim.SetBool("a", true);
            anim.SetBool("d", false);
            mLeft = true;
        }

        if (Input.GetKeyUp("a") && Input.GetKey("d"))
        {
            run = true;
            
            anim.SetBool("d", true);
            anim.SetBool("a", false);
            mLeft = false;
        }

        if (!Input.GetKey("w") && !Input.GetKey("s"))
        {

            anim.SetBool("w", false);
            anim.SetBool("s", false);
        }

        if (!Input.GetKey("a") && !Input.GetKey("d"))
        {

            anim.SetBool("a", false);
            anim.SetBool("d", false);
        }
    }
}
