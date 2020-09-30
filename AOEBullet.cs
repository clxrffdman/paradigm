using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AOEBullet : MonoBehaviour
{
    /*This script will (hopefully) prepare you to get used to Unity's formatting and system for objects.
      
            "gameObject" refers to the current object the script is on, while "GameObject" refers to the GameObject Object.
        
            Parent-Child Hierarchy: gameObject -> transform -> position, etc

            Vector3 = object type containing 3 float values, x,y, and z.

            Vector2 = object type containing 2 float values, x and y.

        Examples: gameObject.transform.position = new Vector3(0,0,0);

            if you use "transform" without the gameObject before it, it defaults to the object the script is attached to.

            gameObject.transform.position = new Vector3(0,0,0); works the same as: transform.position = new Vector3(0,0,0);

        Some commonly used functions/methods between many scripts.

            GameObject.Find("string"); returns the GameObject with the name "string". Use only to find objects that only have one instance and constantly stay loaded. Returns null if object cannot be found. (Will probably cause errors if object cannot be found).

            Destroy(gameObject); takes in a GameObject as an argument, and removes it from the scene, killing all active components and scripts on the object.

            Invoke("string", 0.5f); Runs the function with the name "string", after the amount of seconds in second argument. (For the example, the line will run the function called string after 0.5 seconds).

            Instantiate(gameObject, transform.position, Quaternion.identity); Spawns the GameObject defined in the first argument, at the Vector3 of the second argument, in the orientation of the Quaternion given in the thrid argument.

    */

    //Instance variables (can be defined within script or out of script in the Unity editor)

    
    public float speed;
    public float upSpeed;
    public Transform crosshairTransform;
    public GameObject crosshairObj;
    public GameObject player;
    public GameObject rootShoot;
    private Vector3 target;
    public float vary;
    public float damage;

    //angleOfShot determines the random spread of the shot
    public float angleOfShot;

    public bool friendly;
    public float index;

    public float timeToStop;
    
    
    public bool hasFuse;
    public bool enemyOnly;

    public float pulses;
    public float currentPulse;
    public float pulseDelay;

    public bool canBeHeld;
    public bool inMotion;
    public proxyCursor proxycursor;

    public float currentFuseTime;
    public float fuseTime;

    //selfDamaging determines whether this projectile does self-damage.
    public bool selfDamaging;
    public float selfDamageAmount;

    public ContactPoint2D[] contactPointArray;
    public List<ContactPoint2D> uniqueContacts;

    public float blastRadius;
    public LayerMask damagables;
    public float lifetime;

    public float freezeFrameDuration;
    public float screenShakeMag;
    public float screenShakeDuration;

    public GameObject soundSample;
    public AudioClip detonationSound;
    public float detonationVolume;

    public GameObject lingerEffect;

    //GameObjects of the bullet collision particles, and damage numbers.
    public GameObject bulletDeath;
    public GameObject bulletDeathEnemy;
    public GameObject bulletNumber;
    

    //The bullet's animator component.
    public Animator anim;



    //Runs at the beginning of bullet existence.
    void Start()
    {

        angleOfShot = GameObject.Find("RootShoot").GetComponent<Shoot>().currentGun.transform.rotation.eulerAngles.z;

        //initial variables


        //The GameObject instance variable "crosshairObj" is defined as the GameObject found with the name "CrosshairProxy".
        crosshairObj = GameObject.Find("CrosshairProxy");
        //The instance variable Transform object "crosshairTransform" is defined as the transform of crosshairObj. 
        crosshairTransform = crosshairObj.transform;
        //The GameObject instance variable "player" is defined as the GameObject found with the name "Player".
        player = GameObject.Find("Player");
        rootShoot = GameObject.Find("RootShoot");
        proxycursor = GameObject.Find("CrosshairProxy").GetComponent<proxyCursor>();


        //The transform.right is one of the orientation's of a GameObject. This line points this object's transform.right orientation towards the crosshair.
        transform.right = crosshairTransform.position - transform.position;


        //Add random rotation based on the "vary" float variable.
        transform.Rotate(0, 0, Random.Range(-vary, vary));

        /*
        if(Vector2.Distance(transform.position,player.transform.position) < 0.5f)
        {

        }
        */
        upSpeed = speed;
        if (timeToStop != -1 && !canBeHeld)
        {
            LeanTween.value(gameObject, speed, 0f, timeToStop).setOnUpdate((float val) => {
                upSpeed = val;
            });
        }

        

        if (hasFuse)
        {
            currentFuseTime = fuseTime;
        }
        
        //removes this object from existance.
        Destroy(gameObject, lifetime);
    }


    void Update()
    {
        //The position of the gameObject adds the Vector value of transform.right multiplied by speed and the change in time.

        if (timeToStop != -1 && canBeHeld)
        {
            if (Input.GetKeyUp("f") && !inMotion)
            {

                transform.right = crosshairTransform.position - transform.position;
                LeanTween.value(gameObject, speed, 0f, timeToStop).setOnUpdate((float val) => {
                    upSpeed = val;
                });

                inMotion = true;


            }


        }

        if (canBeHeld)
        {
            if (inMotion)
            {
                transform.position += transform.right * upSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = rootShoot.GetComponent<Shoot>().currentGun.transform.GetChild(0).position;

                if (((proxycursor.proxyAngleOfShot > 0 && proxycursor.proxyAngleOfShot < 180) || (proxycursor.proxyAngleOfShot > 360 && proxycursor.proxyAngleOfShot < 540)))
                {
                    
                    GetComponent<SpriteRenderer>().sortingOrder = -1;

                }
                else
                {

                    GetComponent<SpriteRenderer>().sortingOrder = 1;
                }


            }
        }
        else
        {
            transform.position += transform.right * upSpeed * Time.deltaTime;
        }

        

        if (hasFuse)
        {
            currentFuseTime -= Time.deltaTime;
        }
        

        if(hasFuse && currentFuseTime <= 0)
        {

            hasFuse = false;
            StartCoroutine(Detonation());
        }

        //destroy this object after 3 sec


    }

    public IEnumerator Detonation()
    {

        yield return new WaitForSeconds(pulseDelay);




        currentPulse++;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, blastRadius, damagables);

        Instantiate(bulletDeath, transform.position, Quaternion.identity);


        foreach (Collider2D en in enemies)
        {
            //print(en.gameObject.name);

            if (en.gameObject.tag == "Enemy")
            {
                if (en.gameObject.GetComponent<EnemyBehaviour>() != null)
                {
                    en.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(damage, index);
                }

                if (en.gameObject.GetComponent<MeleeEnemyBehaviour>() != null)
                {
                    en.gameObject.GetComponent<MeleeEnemyBehaviour>().TakeDamage(damage, index);
                }

                if (en.gameObject.GetComponent<ClapEnemyBehaviour>() != null)
                {
                    en.gameObject.GetComponent<ClapEnemyBehaviour>().TakeDamage(damage, index);
                }


                

                var dmgNumber = Instantiate(bulletNumber, en.gameObject.transform.position, Quaternion.identity);
                dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
                if (GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    dmgNumber.GetComponent<BulletNumber>().right = false;
                }
                else
                {
                    dmgNumber.GetComponent<BulletNumber>().right = true;
                }


                //Destroy(gameObject);
            }

            if (selfDamaging && en.gameObject.tag == "Player")
            {
                en.gameObject.GetComponent<PlayerBehaviour>().TakeDamage(selfDamageAmount);

            }

            if (!enemyOnly && (en.gameObject.tag == "Wall" || en.gameObject.tag == "SolidProp"))
            {

                //print "WALL!" (for testing).


                //Spawn an instance of the bulletDeath gameobject at this object's transform's position.
                

                /*
                var dmgNumber = Instantiate(bulletNumber, transform.position, Quaternion.identity);
                dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
                */

                if (en.gameObject.tag == "Wall")
                {
                    if (en.gameObject.GetComponent<wall>() != null)
                    {
                        en.gameObject.GetComponent<wall>().TakeDamage(damage);
                    }

                }


            }


            if (!enemyOnly && en.gameObject.tag == "Destruct")
            {

                en.gameObject.GetComponent<DestRock>().TakeDamage(damage, true);

                

                /*
                var dmgNumber = Instantiate(bulletNumber, transform.position, Quaternion.identity);
                dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
                */



            }

            if (en.gameObject.tag == "CliffCollide")
            {
                if (index != 7)
                {
                    foreach (ContactPoint2D col in uniqueContacts)
                    {
                        
                    }
                }




                /*
                var dmgNumber = Instantiate(bulletNumber, transform.position, Quaternion.identity);
                dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
                */

                
            }

        }

        

        if(pulses > 0)
        {
            if(currentPulse == pulses)
            {
                if (lingerEffect != null)
                {
                    Instantiate(lingerEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(Detonation());

            }
        }
        else
        {
            GameObject.Find("LevelController").GetComponent<PauseMenu>().FreezeFrame(freezeFrameDuration);

            if (detonationSound != null)
            {
                var sound = Instantiate(soundSample, transform.position, Quaternion.identity);
                sound.GetComponent<SoundSample>().SpawnSound(detonationSound, 0f, detonationVolume);
            }

            if (GameObject.Find("Cam") != null && screenShakeDuration != 0 && screenShakeMag != 0)
            {
                GameObject.Find("Cam").GetComponent<CameraShake>().ShakeIt(screenShakeMag, screenShakeDuration);
            }


            if (lingerEffect != null)
            {
                Instantiate(lingerEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        
    }


    //runs when 2D collider on bullet collides with something. The collider the bullet collides with is definied as "other".
    void OnCollisionEnter2D(Collision2D other)
    {
        if(pulses <= 0)
        {
            
            StartCoroutine(Detonation());
        }
        


    }



    



    void BulletDeath()
    {
        Destroy(gameObject);
    }


}
