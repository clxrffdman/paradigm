using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyBehaviour : MonoBehaviour
{
    public Rigidbody2D rb;
    public float health;
    public float armor;
    public Animator anim;
    public bool alive;

    public bool active;

    public int index;
    public bool detected;

    public float moveSpeed;

    public bool changeState;
    public float playerRotation;
    public float moveDirection;
    public RaycastHit2D ray;
    public bool left;
    public float direction;
    public AudioSource aud;

    public float minRange;

    public float currentTime;
    public float attackTime;

    public float moveRate;
    public float moveRateMax;

    public bool sprited;

    public bool patrolling;

    public float attackCharge;
    public GameObject[] drops;

    public bool invincible;

    public GameObject currentBulletSpawn;

    public LayerMask hittable;

    public float patrolDuration;
    public float patrolDurationMax;
    public float patrolPause;
    public float patrolPauseMax;

    // Start is called before the first frame update
    void Start()
    {
        sprited = false;
        aud = GetComponent<AudioSource>();
        hittable = transform.GetChild(0).transform.GetChild(0).GetComponent<EnemyDetection>().whatToHit;
        rb = GetComponent<Rigidbody2D>();
        changeState = true;
        anim = GetComponent<Animator>();

        GetComponent<IsAlive>().alive = true;
        alive = true;
        patrolling = false;
        GetComponent<CircleCollider2D>().enabled = true;
        StartCoroutine(AIBehaviour());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0 && alive)
        {
            Death();
        }

       
        
    }

    void OnEnable()
    {
        StopCoroutine(AIBehaviour());
        StartCoroutine(AIBehaviour());
        changeState = true;
    }

    public IEnumerator AIBehaviour()
    {
        while(alive)
        {
            if (alive)
            {
                if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) < minRange)
                {
                    Stop();
                }
                else if (currentTime > 0)
                {

                    Stop();
                }
            }

            ray = Physics2D.Raycast(transform.position, GameObject.Find("Player").transform.position, 2f, hittable);

            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }


            if (index == 1 && changeState)
            {

                if (!detected)
                {
                    CancelInvoke();
                    changeState = false;
                    if(patrolling == false)
                    {
                        patrolling = true;
                        StartCoroutine(Patrol());
                    }
                    
                }

                if (detected)
                {
                    patrolling = false;
                    DetectSprite();
                    transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().detectRange = transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().visibilityRange;
                    changeState = false;
                    
                    CancelInvoke();
                    StartCoroutine(RandomChase());

                    if (transform.GetChild(0).GetComponent<SimpleTurret>() != null)
                    {
                        transform.GetChild(0).GetComponent<SimpleTurret>().alerted = true;
                    }

                    if (transform.GetChild(0).GetComponent<MeleeEnemyAI>() != null)
                    {
                        transform.GetChild(0).GetComponent<MeleeEnemyAI>().alerted = true;
                    }


                }
            }
            yield return new WaitForSeconds(0.05f + Random.Range(0,0.02f));
        }

        

    }



    public IEnumerator Patrol()
    {
        while (patrolling && alive)
        {
            float randomAngle = Random.Range(0, 361);
            rb.velocity = new Vector2((moveSpeed * Mathf.Cos(randomAngle * Mathf.Deg2Rad)), (moveSpeed * Mathf.Sin(randomAngle * Mathf.Deg2Rad)));

            yield return new WaitForSeconds(Random.Range(patrolDuration,patrolDurationMax));
            Stop();
            yield return new WaitForSeconds(Random.Range(patrolPause,patrolPauseMax));
        }
        

        
    }

    
    public IEnumerator RandomChase()
    {
        while (alive)
        {
            yield return new WaitForSeconds(attackCharge);


            if (transform.GetChild(0).GetComponent<MeleeEnemyAI>() != null)
            {
                transform.GetChild(0).GetComponent<MeleeEnemyAI>().SpawnBullet();
            }


            playerRotation = transform.GetChild(0).rotation.eulerAngles.z;
            if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().visible)
            {
                rb.velocity = new Vector2((moveSpeed * Mathf.Cos((playerRotation + Random.Range(0, 10)) * Mathf.Deg2Rad)), (moveSpeed * Mathf.Sin((playerRotation + Random.Range(0, 10)) * Mathf.Deg2Rad)));
            }
            else
            {


                if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 1)
                {

                    direction = Random.Range(10, 81);
                }

                if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 2)
                {
                    direction = Random.Range(100, 171);
                }

                if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 3)
                {
                    direction = Random.Range(190, 261);
                }

                if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 4)
                {
                    direction = Random.Range(280, 351);
                }


                //direction = playerRotation + Random.Range(-30,30);

                if (Vector2.Distance(transform.position, ray.point) < 0.5f)
                {
                    direction += 180f;
                }

                rb.velocity = new Vector2(((moveSpeed + Random.Range(0, 0.5f)) * Mathf.Cos(direction * Mathf.Deg2Rad)), ((moveSpeed + Random.Range(0, 0.5f)) * Mathf.Sin(direction * Mathf.Deg2Rad)));
            }



            yield return new WaitForSeconds(Random.Range(1, 2));
            Stop();
        }

        yield return new WaitForSeconds(Random.Range(moveRate,moveRateMax));



    }
    
    /*
    void RandomChase()
    {

        if(transform.GetChild(0).GetComponent<MeleeEnemyAI>() != null)
        {
            transform.GetChild(0).GetComponent<MeleeEnemyAI>().SpawnBullet();
        }
        

        playerRotation = transform.GetChild(0).rotation.eulerAngles.z;
        if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().visible)
        {
            rb.velocity = new Vector2((moveSpeed * Mathf.Cos((playerRotation + Random.Range(0,10)) * Mathf.Deg2Rad)), (moveSpeed * Mathf.Sin((playerRotation + Random.Range(0, 10)) * Mathf.Deg2Rad)));
        }
        else
        {
            
            
            if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 1)
            {
                
                direction = Random.Range(10,81);
            }

            if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 2)
            {
                direction = Random.Range(100, 171);
            }

            if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 3)
            {
                direction = Random.Range(190, 261);
            }

            if (transform.GetChild(0).GetChild(0).GetComponent<EnemyDetection>().playerQuadrant == 4)
            {
                direction = Random.Range(280, 351);
            }
            

            //direction = playerRotation + Random.Range(-30,30);

            if (Vector2.Distance(transform.position, ray.point) < 0.5f)
            {
                direction += 180f;
            }

            rb.velocity = new Vector2(((moveSpeed + Random.Range(0,0.5f)) * Mathf.Cos(direction * Mathf.Deg2Rad)), ((moveSpeed + Random.Range(0, 0.5f)) * Mathf.Sin(direction * Mathf.Deg2Rad)));
        }
        
        
        
            Invoke("Stop", Random.Range(1, 2));
        

        
    }

    */

    void Stop()
    {
        /*
        if(currentBulletSpawn != null)
        {
            Destroy(currentBulletSpawn); 
        }
        */

        rb.velocity = new Vector2(0,0);
        if(alive == false)
        {
            /*

            LeanTween.value(gameObject, 0.05f, 0f, 0.25f).setOnUpdate((float val) => {
                GetComponent<Light2D>().intensity = val;
            });

            */

            Invoke("StopAnimAudio", 1f);
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    void Slow()
    {
        if(rb.velocity != new Vector2(0, 0))
        {
            /*
            LeanTween.value(gameObject, 0.05f, 0f, 0.25f).setOnUpdate((float val) => {
                GetComponent<Light2D>().intensity = val;
            });
            */
        }

        rb.velocity = Vector2.Lerp(rb.velocity,new Vector2(0,0),0.05f);

    }

    void StopAnimAudio()
    {
        aud.enabled = false;
        anim.enabled = false;
        //GetComponent<Light2D>().enabled = false;
    }

    

    public void TakeDamage(float damage, float gunindex)
    {
        if (alive && !invincible)
        {
            health -= damage;

            if (currentBulletSpawn != null)
            {
                Destroy(currentBulletSpawn);
            }

            if (transform.GetChild(0).GetComponent<SimpleTurret>() != null)
            {
                transform.GetChild(0).GetComponent<SimpleTurret>().currentAttackCooldown = transform.GetChild(0).GetComponent<SimpleTurret>().attackCooldown;
            }

            if (!detected)
            {
                changeState = true;
                detected = true;
            }


            if (anim.enabled)
            {
                anim.Play("getHit");
            }

            


            GameObject.Find("LevelController").GetComponent<PauseMenu>().FreezeFrame(GameObject.Find("RootShoot").GetComponent<Shoot>().currentFreezeFrame);


            if (index == 1 && gunindex == 6)
            {
                aud.Stop();
                aud.clip = GetComponent<AudioManager>().audClips[1];
                aud.Play();
            }
        }

    }

    public void ForcedDeath()
    {
        if (currentBulletSpawn != null)
        {
            Destroy(currentBulletSpawn);
        }

        if (index == 1)
        {

            CancelInvoke();
            if (GetComponentInChildren<SimpleTurret>() != null)
            {
                GetComponentInChildren<SimpleTurret>().ClearInvoke();
            }


            if (GetComponentInChildren<MeleeEnemyAI>() != null)
            {
                GetComponentInChildren<MeleeEnemyAI>().ClearInvoke();
            }

            

            if (anim.enabled)
            {
                anim.Play("death");
            }


            
            rb.velocity = new Vector2(0f, -1.7f);


            

            alive = false;
            GetComponent<IsAlive>().alive = false;
            Invoke("Stop", 0.275f);
            GetComponentInChildren<SimpleTurret>().enabled = false;
            GetComponent<EnemyBehaviour>().enabled = false;
        }
    }

    void Death()
    {
        
        GameObject.Find("Player").GetComponent<DashAbility>().timeLeft = 0;
        
        GameObject.Find("Player").GetComponent<DashAbility>().teleCooldownSprite.GetComponent<Animator>().SetBool("OnCooldown", false);
        GameObject.Find("Player").GetComponent<DashAbility>().teleCooldownSprite.GetComponent<Animator>().Play("TeleChargeIdleFull");

        if (currentBulletSpawn != null)
        {
            Destroy(currentBulletSpawn);
        }

        if (index == 1)
        {
            
            CancelInvoke();
            if(GetComponentInChildren<SimpleTurret>() != null)
            {
                GetComponentInChildren<SimpleTurret>().ClearInvoke();
            }


            if (GetComponentInChildren<MeleeEnemyAI>() != null)
            {
                GetComponentInChildren<MeleeEnemyAI>().ClearInvoke();
            }

            

            if (anim.enabled)
            {
                anim.Play("death");
            }
            
            rb.velocity = new Vector2(0f, -1.7f);


            PlayDeathSound();
            
            for(int i = 0; i < Random.Range(1, 3); i++)
            {
                GameObject drop = Instantiate(drops[0], transform.position, Quaternion.identity);
                drop.transform.parent = GameObject.Find("Player").GetComponent<PlayerBehaviour>().currentObjectArray.transform;
                
                drop.GetComponent<Interactable>().target = (Random.insideUnitCircle * 1 + new Vector2(transform.position.x, transform.position.y)) - new Vector2(transform.position.x, transform.position.y);


            }

            alive = false;
            GetComponent<IsAlive>().alive = false;
            Invoke("Stop", 0.275f);
            GetComponentInChildren<SimpleTurret>().enabled = false;
            GetComponent<EnemyBehaviour>().enabled = false;
        }
        
        //Invoke("DestroyGameObject", 0.5f);

    }

    
    void PlayDeathSound()
    {
        if(index == 1)
        {
            aud.clip = GetComponent<AudioManager>().audClips[0];
            aud.Play();
        }
    }
    

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    void DetectSprite()
    {
        if(sprited == false)
        {
            sprited = true;
            LeanTween.color(transform.GetChild(1).gameObject, new Color(1, 1, 1, 1), 0.2f);
            LeanTween.color(transform.GetChild(1).gameObject, new Color(0, 0, 0, 0), 0.1f).setDelay(0.5f);
            LeanTween.scale(transform.GetChild(1).gameObject, new Vector3(0.66f, 0.66f, 0.66f), 0.15f);
            LeanTween.scale(transform.GetChild(1).gameObject, new Vector3(0f, 0f, 0f), 0.15f).setDelay(0.5f);
        }
        
    }
}
