using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyLaser : MonoBehaviour
{
    public float maxRange = 100f;
    private LineRenderer line;
    public RaycastHit2D ray;
    public GameObject hostEnemy;


    public RaycastHit2D[] rayArray;

    public GameObject rootShoot;
    public float distance;
    public LayerMask[] hitableLayers;
    public LayerMask whatToHit;
    public float shootAngle;

    public float damage;

    public bool tracking;
    public float trackDelayToShot;
    public float hesitateDelay;

    public bool lingering;

    public float strength;
    public GameObject player;
    float str;

    public GameObject bulletDeath;
    public GameObject bulletDeathEnemy;

    public GameObject soundSample;
    public Quaternion targetRotation;

    public float startLineWidth;
    public float endLineWidth;

    public float postStartLineWidth;
    public float postEndLineWidth;

    public float thinTime;

    public AudioClip shootsound;

    public bool hit;
    public int end;
    public bool block;

    void Start()
    {
        tracking = true;
        StartCoroutine(DamageBeam(trackDelayToShot));
        player = GameObject.Find("Player");
        hostEnemy.GetComponentInParent<EnemyBehaviour>().currentBulletSpawn = gameObject;
        transform.LookAt(player.transform);

    }



    void FixedUpdate()
    {
        transform.position = hostEnemy.transform.position;
        line.SetPosition(0, hostEnemy.transform.position);
        if (tracking)
        {
            

            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            str = Mathf.Min(strength * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

            StartBeaming();

        }

        
    }


    public void StartBeaming()
    {


        //transform.LookAt(GameObject.Find("Player").transform);
        rayArray = Physics2D.RaycastAll(hostEnemy.transform.position, transform.forward, distance, whatToHit);


        hit = false;
        for (int i = 0; i < rayArray.Length; i++)
        {
            if (rayArray[i].collider != null)
            {
                hit = true;
            }
        }

        if (hit)
        {

            for (int i = 0; i < rayArray.Length; i++)
            {
                if (rayArray[i].collider != null)
                {
                    if (rayArray[i].collider.tag == "Destruct" || rayArray[i].collider.tag == "CliffCollide" || rayArray[i].collider.tag == "Wall" || rayArray[i].collider.tag == "PlayerBullet")
                    {
                        end = i;
                        i = rayArray.Length;
                        block = true;
                    }
                    else
                    {
                        end = 0;
                        block = false;
                    }

                }
            }

            line.enabled = true;
            line.SetPosition(0, hostEnemy.transform.position);
            if(block)
            {
                line.SetPosition(1, rayArray[end].point);
            }
            else
            {
                line.SetPosition(1, transform.position + transform.forward.normalized * distance);
            }
            

            //line.material.SetTextureScale("_Diffuse", new Vector2(5, 1));

            //print("LASERTEST");
            /*
            for (int i = 0; i <= end; i++)
            {
                if (rayArray[i].collider != null)
                {
                    if (rayArray[i].collider.tag == "Destruct")
                    {
                        rayArray[i].collider.GetComponent<DestRock>().TakeDamage(damage);
                        //Instantiate(bulletDeath, ray.point, Quaternion.identity);
                    }
                    else if (rayArray[i].collider.tag == "Wall")
                    {
                        rayArray[i].collider.GetComponent<wall>().TakeDamage(damage);
                        //Instantiate(bulletDeath, ray.point, Quaternion.identity);

                    }
                    else if (rayArray[i].collider.tag == "Player")
                    {
                        Invoke("LineToWhite", 0.1f);
                        rayArray[i].collider.GetComponent<PlayerBehaviour>().TakeDamage(damage);

                        //Instantiate(bulletDeathEnemy, ray.point, Quaternion.identity);



                    }
                    else if (rayArray[i].collider != null)
                    {
                        //Instantiate(bulletDeath, ray.point, Quaternion.identity);
                    }
                }
            }

            */


        }
        if (!hit)
        {

            line.enabled = true;
            line.SetPosition(0, hostEnemy.transform.position);
            line.SetPosition(1, transform.position + transform.forward.normalized * distance);
            

        }
        
    }

    

    public IEnumerator DamageBeam(float delay)
    {
        bool isRepeating = true;

        

        while (isRepeating)
        {

            yield return new WaitForSeconds(delay);

            if (!lingering)
            {
                tracking = false;
            }
            


            hostEnemy.GetComponentInParent<Animator>().Play("attack");
            yield return new WaitForSeconds(hesitateDelay);

            var sound = Instantiate(soundSample, transform.position, Quaternion.identity);


            AudioClip currentSound = shootsound;
            sound.GetComponent<SoundSample>().SpawnSound(currentSound, 0f, 0.8f);

            line.startColor = new Color(1, 1f, 1f, 1f);
            line.endColor = new Color(1, 1f, 1f, 1f);

            rayArray = Physics2D.RaycastAll(transform.position, transform.forward, distance, whatToHit);

            line.startWidth = postStartLineWidth;
            line.endWidth = postEndLineWidth;



            for (int i = 0; i < rayArray.Length; i++)
            {
                if (rayArray[i].collider != null)
                {
                    hit = true;
                }
            }

            if (hit)
            {

                for (int i = 0; i < rayArray.Length; i++)
                {
                    if (rayArray[i].collider != null)
                    {
                        if (rayArray[i].collider.tag == "Destruct" || rayArray[i].collider.tag == "CliffCollide" || rayArray[i].collider.tag == "Wall" || rayArray[i].collider.tag == "PlayerBullet")
                        {
                            end = i;
                            i = rayArray.Length;
                            block = true;
                        }
                        else
                        {
                            end = 0;
                            block = false;
                        }


                    }
                }

                if (!lingering)
                {
                    InvokeRepeating("ThinLine", 0f, 0.01f);
                    Invoke("KillLine", 0.2f);
                }
                
                

                line.enabled = true;
                line.SetPosition(0, hostEnemy.transform.position);

                if (block)
                {
                    line.SetPosition(1, rayArray[end].point);
                }
                else
                {
                    line.SetPosition(1, transform.position + transform.forward.normalized * distance);
                }

                //print("LASERTEST");

                for (int i = 0; i <= end; i++)
                {
                    if (rayArray[i].collider != null)
                    {
                        if (rayArray[i].collider.tag == "Destruct")
                        {
                            rayArray[i].collider.GetComponent<DestRock>().TakeDamage(damage, false);
                            //Instantiate(bulletDeath, ray.point, Quaternion.identity);
                        }
                        else if (rayArray[i].collider.tag == "Wall")
                        {
                            rayArray[i].collider.GetComponent<wall>().TakeDamage(damage);
                            //Instantiate(bulletDeath, ray.point, Quaternion.identity);

                        }
                        else if (rayArray[i].collider.tag == "Player")
                        {
                            Invoke("LineToWhite", 0.1f);
                            rayArray[i].collider.GetComponent<PlayerBehaviour>().TakeDamage(damage);

                            //Instantiate(bulletDeathEnemy, ray.point, Quaternion.identity);



                        }
                        else if (rayArray[i].collider != null)
                        {
                            //Instantiate(bulletDeath, ray.point, Quaternion.identity);
                        }
                    }
                }




            }
            if (!hit)
            {

                line.enabled = true;
                line.SetPosition(0, hostEnemy.transform.position);
                line.SetPosition(1, hostEnemy.transform.position + transform.forward.normalized * distance);
                if (!lingering)
                {
                    InvokeRepeating("ThinLine", 0f, 0.001f);
                    Invoke("KillLine", 0.15f);
                }
                

            }





            if (lingering)
            {
                isRepeating = true;
            }
            else
            {
                isRepeating = false;
            }
        }
        

        

        
    }

    

    

    void Awake()
    {
        tracking = true;
        line = GetComponent<LineRenderer>();
        line.enabled = true;
        rootShoot = GameObject.Find("RootShoot");
        for (int i = 0; i < hitableLayers.Length; i++)
        {
            whatToHit = whatToHit + hitableLayers[i];
        }

       

        line.positionCount = 2;

        line.startColor = new Color(1, 1f, 1f, 0.5f);
        line.endColor = new Color(1, 1f, 1f, 0.5f);

        line.startWidth = startLineWidth;
        line.endWidth = endLineWidth;
    }

    void LineToWhite()
    {
        line.startColor = new Color(0.6f, 0.6f, 0.6f, 1);
        line.endColor = new Color(0.6f, 0.6f, 0.6f, 1f);
    }

    


    void ThinLine()
    {
        line.startWidth = Mathf.Lerp(line.startWidth, 0, thinTime);
        line.endWidth = Mathf.Lerp(line.endWidth, 0, thinTime);
    }

    void KillLine()
    {
        Destroy(gameObject);
    }
}