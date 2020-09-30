using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float moveSpeedY;
    public float moveSpeedInitial;
    public float moveSpeedInitialY;
    public float moveSpeedSprint;
    public float moveSpeedSprintY;

    public float walkTime;
    public float currentTime;

    public bool interactEnable;

    public Rigidbody2D rb;
    public Vector3 movement;
    public float drag;
    public int[] ad = new int[2];
    public bool left;
    public bool up;

    public bool isMoving;

    public bool isShoot;
    public bool midTele;

    public GameObject crosshairProxy;

    public float meleeDashSpeed;
    public float meleeDashDuration;
    public float meleeMoveCooldown;

    public GameObject teleFX;

    public GameObject playerShadow;

    public bool moveEnabled;

    public GameObject soundSample;

    public AudioClip[] walkSounds;

    

    // Start is called before the first frame update
    void Start()
    {
        interactEnable = true;
        midTele = false;
        moveEnabled = false;
        moveSpeedInitial = moveSpeed;
        moveSpeedY = moveSpeedInitialY;
        
        
        ad[0] = 0;
        ad[1] = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (moveEnabled)
        {
            Move();
        }
        
        //Sprint();

    }

    public void StopMove()
    {
        
        moveEnabled = false;
        rb.velocity = new Vector2(0, 0);
    }

    public IEnumerator SwingMove()
    {
        StopMove();
        
        //GetComponent<PlayerAnim>().ReturnToIdle();
        rb.velocity = new Vector3(meleeDashSpeed * Mathf.Cos(Mathf.Deg2Rad * crosshairProxy.GetComponent<proxyCursor>().proxyAngleOfShot), meleeDashSpeed * Mathf.Sin(Mathf.Deg2Rad * crosshairProxy.GetComponent<proxyCursor>().proxyAngleOfShot), 0f);
        crosshairProxy.GetComponent<proxyCursor>().checkAngle = false;
        //transform.position = Vector3.MoveTowards(transform.position, crosshairProxy.GetComponent<Rigidbody>().position, 0.2f);
        yield return new WaitForSecondsRealtime(meleeDashDuration);
        StopMove();
        ResetMove();
        
        yield return new WaitForSecondsRealtime(meleeMoveCooldown - meleeDashDuration);
        crosshairProxy.GetComponent<proxyCursor>().checkAngle = true;
        //GetComponent<PlayerAnim>().ReturnToChecking();
        StartCoroutine(StartMove(0.01f));

    }

    public IEnumerator StartMove(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        moveEnabled = true;
    }

    public void ResetMove()
    {
        if (Input.GetKey("a") && !Input.GetKey("d"))
        {

            left = true;
        }

        if (Input.GetKey("d") && !Input.GetKey("a"))
        {

            left = false;
        }

        if (Input.GetKey("d") && Input.GetKey("a"))
        {

            if (left)
            {
                left = true;
            }
            else
            {
                left = false;

            }
        }

        if (Input.GetKey("w") && !Input.GetKey("s"))
        {

            up = true;
        }

        if (Input.GetKey("s") && !Input.GetKey("w"))
        {

            up = false;
        }

        if (Input.GetKey("w") && Input.GetKey("s"))
        {

            if (up)
            {
                up = true;
            }
            else
            {
                up = false;

            }
        }
    }

    void Move()
    {

        /*if (Input.GetKey("a"))
        {
            ad[1] = 1;
            if(ad[0] == 0)
            {
                ad[0] = 1;
                
            }

            

        }

        if (Input.GetKeyUp("a"))
        {
            for (int i = 0; i < 2; i++)
            {
                if (ad[i] == 1)
                {
                    ad[i] = 0;
                }

            }

        }


        if (Input.GetKey("d"))
        {
            ad[1] = 2;
            if (ad[0] == 0)
            {
                ad[0] = 2;
                
            }

            

        }

        if (Input.GetKeyUp("d"))
        {
            for (int i = 0; i < 2; i++)
            {
                if (ad[i] == 2)
                {
                    ad[i] = 0;
                }

            }

        }*/

        if (Input.GetKeyDown("a"))
        {

            left = true;
        }

        if (Input.GetKeyDown("d"))
        {

            left = false;
        }

        if (Input.GetKeyUp("d") && Input.GetKey("a"))
        {

            left = true;
        }

        if (Input.GetKeyUp("a") && Input.GetKey("d"))
        {

            left = false;
        }

        if (Input.GetKey("d") && !Input.GetKey("a"))
        {
            left = false;
        }

        if (Input.GetKey("a") && !Input.GetKey("d"))
        {
            left = true;
        }


        if (left)
        {
            movement.x = -moveSpeed;
            isMoving = true;
        }
        if (!left)
        {
            movement.x = moveSpeed;
            isMoving = true;
        }

        if (!Input.GetKey("a") && !Input.GetKey("d"))
        {
            movement.x = 0;
            
        }


        /////////////////////////////////////////
        ///



        if (Input.GetKeyDown("w"))
        {

            up = true;
        }

        if (Input.GetKeyDown("s"))
        {

            up = false;
        }

        if (Input.GetKeyUp("s") && Input.GetKey("w"))
        {

            up = true;
        }

        if (Input.GetKeyUp("w") && Input.GetKey("s"))
        {

            up = false;
        }


        if (up)
        {
            movement.y = moveSpeedY;
            isMoving = true;
        }
        if (!up)
        {
            movement.y = -moveSpeedY;
            isMoving = true;
        }

        if (!Input.GetKey("w") && !Input.GetKey("s"))
        {
            movement.y = 0;
            
        }

        if (!Input.GetKey("w") && !Input.GetKey("s") && !Input.GetKey("a") && !Input.GetKey("d"))
        {
            isMoving = false;
        }


        //movement.x = -moveSpeed;
        //movement.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;


        //if (Input.GetKey("d"))
        //{

        //movement.x = +moveSpeed;
        //}








        //if (Input.GetKey("d"))
        //{
        //movement.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

        //ad[1] = "d";

        //if (string.Compare(ad[0],"") == 0)
        //{
        //ad[0] = "d";
        //}



        //}


        



        

        /*if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
        {
            movement.y = 0f;
            movement.x = 0f;
        }
        */

        if (movement.x != 0)
        {

            movement.x = (float)Mathf.Lerp(movement.x, 0, drag);
        }

        if (movement.y != 0)
        {

            movement.y = (float)Mathf.Lerp(movement.y, 0, drag);
        }


        //STOP MOVING WHILE SHOOTING BELOW
        /*
        if (Input.GetKey(KeyCode.Mouse0))
        {
            movement.x = 0;
            movement.y = 0;
        }
        */

        if (midTele)
        {
            movement.x = 0f;
            movement.y = 0f;
        }

        if(new Vector2(movement.x, movement.y) != Vector2.zero && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        if (currentTime <= 0)
        {

            var sound = Instantiate(soundSample, transform.position, Quaternion.identity);

            
            AudioClip currentSound = walkSounds[0];
            sound.GetComponent<SoundSample>().SpawnSound(currentSound, 0f, .05f);
            currentTime = walkTime;
        }



        



    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x, movement.y);
    }


    public IEnumerator Intro()
    {
        midTele = true;
        moveEnabled = false;
        playerShadow.SetActive(false);

        GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);

        Animator teleAnim = teleFX.GetComponent<Animator>();

        teleFX.GetComponent<SpriteRenderer>().enabled = true;
        teleAnim.SetBool("TeleTime", true);
        yield return new WaitForSecondsRealtime(0.7f);
        teleFX.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        teleFX.GetComponent<SpriteRenderer>().enabled = true;
        teleAnim.SetBool("TeleTime", true);
        yield return new WaitForSecondsRealtime(0.7f);
        teleFX.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        teleFX.GetComponent<SpriteRenderer>().enabled = true;
        teleAnim.SetBool("TeleTime", true);
        yield return new WaitForSecondsRealtime(0.7f);
        teleFX.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        


        LeanTween.value(gameObject, new Color(1,1,1,0), new Color(1,1,1,1), 0.15f).setOnUpdate((Color val) => {
            GetComponent<SpriteRenderer>().color = val;
        });
        playerShadow.SetActive(true);
        midTele = false;
        moveEnabled = true;



    }

    

    void Sprint()
    {

        if (Input.GetKey("left shift"))
        {
            moveSpeedY = moveSpeedSprintY;
            moveSpeed = moveSpeedSprint;

        }
        else
        {
            moveSpeedY = moveSpeedInitialY;
            moveSpeed = moveSpeedInitial;
        }
    }
}
