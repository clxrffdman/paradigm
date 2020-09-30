using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
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

    //Variables (can be defined within script or out of script in the Unity editor) most are public for testing purposes.

    public float speed;
    public float upSpeed;
    public Transform crosshairTransform;
    public GameObject crosshairObj;
    public GameObject player;
    private Vector3 target;
    public float vary;
    public float damage;
    public float angleOfShot;
    public bool friendly;
    public float index;
    public float lifetime;
    public bool followPlayer;
    public bool killsEnemyBullets;

    public bool isBarrier;
    public bool bouncing;

    public ContactPoint2D[] contactPointArray;
    public List<ContactPoint2D> uniqueContacts;

    public bool invincible;

    public float timeToStop;

    //GameObjects of the bullet collision particles, and damage numbers.
    public GameObject bulletDeath;
    public GameObject bulletDeathEnemy;
    public GameObject bulletNumber;

    //The bullet's animator component.
    public Animator anim;

    public GameObject detonateObject;

    public bool bouncingPhysics;
    public Rigidbody2D rb;
    
    //Runs at the beginning of bullet existence.
    void Start()
    {
        
        angleOfShot = GameObject.Find("RootShoot").GetComponent<Shoot>().currentGun.transform.rotation.eulerAngles.z;

        //initial variables
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        //The GameObject instance variable "crosshairObj" is defined as the GameObject found with the name "CrosshairProxy".
        crosshairObj = GameObject.Find("CrosshairProxy");
        //The instance variable Transform object "crosshairTransform" is defined as the transform of crosshairObj. 
        crosshairTransform = crosshairObj.transform;
        //The GameObject instance variable "player" is defined as the GameObject found with the name "Player".
        player = GameObject.Find("Player");

        //The transform.right is one of the orientation's of a GameObject. This line points this object's transform.right orientation towards the crosshair.
        transform.right = crosshairTransform.position - transform.position;
        

        //Add random rotation based on the "vary" float variable.
        transform.Rotate(0,0, Random.Range(-vary, vary));

        /*
        if(Vector2.Distance(transform.position,player.transform.position) < 0.5f)
        {

        }
        */

        upSpeed = speed;
        if (timeToStop != -1)
        {
            LeanTween.value(gameObject, speed, 0f, timeToStop).setOnUpdate((float val) => {
                upSpeed = val;
            });
        }
        if (isBarrier)
        {
            StartCoroutine(BarrierPhase());
        }

        rb.AddForce((new Vector2(transform.right.x, transform.right.y) * upSpeed),ForceMode2D.Impulse);

        //removes this object from existance.
        if(detonateObject != null)
        {
            Invoke("SummonDetonate", lifetime);
        }
        
        Destroy(gameObject, lifetime);
        
    }


    void Update()
    {
        //The position of the gameObject adds the Vector value of transform.right multiplied by speed and the change in time.
        if (followPlayer)
        {
            transform.position = player.transform.position;
        }
        else
        {
            if (bouncingPhysics)
            {
                rb.velocity = upSpeed * (rb.velocity.normalized);



            }
            else
            {
                transform.position += transform.right * upSpeed * Time.deltaTime;
            }

            
        }
            
        
        
        //destroy this object after 3 sec
        

    }

    public void UpOrDown(bool up)
    {
        if (up)
        {
            GetComponent<Animator>().SetBool("up", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("up", false);
        }
    }

    //runs when 2D collider on bullet collides with something. The collider the bullet collides with is definied as "other".
    void OnCollisionEnter2D(Collision2D other)
    {
        if (bouncingPhysics)
        {
            upSpeed += 0.5f;
        }

        contactPointArray = new ContactPoint2D[other.contactCount];

        other.GetContacts(contactPointArray);

        uniqueContacts = new List<ContactPoint2D>();

        for(int i = 0; i < contactPointArray.Length; i++)
        {
            bool found = false;
            for(int j = 0; j < uniqueContacts.Count; j++)
            {

                if(uniqueContacts[j].collider.gameObject == contactPointArray[i].collider.gameObject)
                {
                    found = true;
                }
                
            }
            if (!found)
            {
                uniqueContacts.Add(contactPointArray[i]);
            }

        }

        //if the GameObject associated with "other", has a tag with the name "Wall" or a tag with the name "SolidProp"...
        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "SolidProp")
        {

            //print "WALL!" (for testing).


            //Spawn an instance of the bulletDeath gameobject at this object's transform's position.
            if (index != 7)
            {
                foreach (ContactPoint2D col in uniqueContacts)
                {
                    
                    Instantiate(bulletDeath, col.point, Quaternion.identity);
                }
            }

            /*
            var dmgNumber = Instantiate(bulletNumber, transform.position, Quaternion.identity);
            dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
            */

            if (other.gameObject.tag == "Wall")
            {
                if(other.gameObject.GetComponent<wall>() != null)
                {
                    other.gameObject.GetComponent<wall>().TakeDamage(damage);
                    if (index == 7)
                    {
                        foreach (ContactPoint2D col in uniqueContacts)
                        {
                            Instantiate(bulletDeath, col.point, Quaternion.identity);
                        }
                    }
                }
                
                
            }

            if (!invincible)
            {
                Destroy(gameObject);
            }
            

        }

        if (killsEnemyBullets && other.gameObject.tag == "Enemy Bullet")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Destruct")
        {
            
            other.gameObject.GetComponent<DestRock>().TakeDamage(damage, true);

            foreach (ContactPoint2D col in uniqueContacts)
            {
                Instantiate(bulletDeath, col.point, Quaternion.identity);
            }

            /*
            var dmgNumber = Instantiate(bulletNumber, transform.position, Quaternion.identity);
            dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
            */

            if (!invincible)
            {
                Destroy(gameObject);
            }

        }

        if (other.gameObject.tag == "Enemy")
        {
            if(other.gameObject.GetComponent<EnemyBehaviour>() != null)
            {
                other.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(damage, index);
            }

            if (other.gameObject.GetComponent<MeleeEnemyBehaviour>() != null)
            {
                other.gameObject.GetComponent<MeleeEnemyBehaviour>().TakeDamage(damage, index);
            }

            if (other.gameObject.GetComponent<ClapEnemyBehaviour>() != null)
            {
                other.gameObject.GetComponent<ClapEnemyBehaviour>().TakeDamage(damage, index);
            }


            foreach (ContactPoint2D col in uniqueContacts)
            {
                Instantiate(bulletDeathEnemy, col.point, Quaternion.identity);
            }

            var dmgNumber = Instantiate(bulletNumber, transform.position, Quaternion.identity);
            dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
            if(GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                dmgNumber.GetComponent<BulletNumber>().right = false;
            }
            else
            {
                dmgNumber.GetComponent<BulletNumber>().right = true;
            }


            if (!invincible)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == "CliffCollide")
        {
            if(index != 7)
            {
                foreach (ContactPoint2D col in uniqueContacts)
                {
                    Instantiate(bulletDeath, col.point, Quaternion.identity);
                }
            }
            

            

            /*
            var dmgNumber = Instantiate(bulletNumber, transform.position, Quaternion.identity);
            dmgNumber.GetComponent<BulletNumber>().SetVal(damage);
            */

            if (!invincible)
            {
                Destroy(gameObject);
            }
        }


        if (other.gameObject.tag == "EnviroWallSafe")
        {
            //GetComponent<CircleCollider2D>().enabled = false;

        }

        /*
        Tilemap tilemap = GameObject.Find("TilemapEx").GetComponent<Tilemap>();
        Vector3 hitPosition = Vector3.zero;
        foreach (ContactPoint2D hit in other.contacts)
        {
            Debug.Log(hit.point);
            hitPosition.x = hit.point.x - 0.1f;
            hitPosition.y = hit.point.y - 0.1f;
            Vector3Int cell = new Vector3Int((int)hitPosition.x, (int)hitPosition.y, 0);
            if (tilemap.GetTile(tilemap.WorldToCell(hitPosition)))
            {
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
            }
            
        }
        */

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerBullet")
        {
            friendly = true;

        }

        if (other.gameObject.tag != "Player" && other.gameObject.tag != "PlayerBullet")
        {
            friendly = false;
            
        }

        if (other.gameObject.tag == "EnviroWallSafe")
        {
            friendly = true;

        }

        if (!friendly)
        {
            //Instantiate(bulletDeath, transform.position, Quaternion.identity);
            Invoke("BulletDeath", 0f);
        }
        
        
    }




    


    public IEnumerator BarrierPhase()
    {
        yield return new WaitForSeconds(3.5f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        GetComponent<SpriteRenderer>().enabled = false;


    }

    void SummonDetonate()
    {
        Instantiate(detonateObject, transform.position, Quaternion.identity);
    }

    void BulletDeath()
    {
        if (!invincible)
        {
            Destroy(gameObject);
        }
        
    }

    
}
