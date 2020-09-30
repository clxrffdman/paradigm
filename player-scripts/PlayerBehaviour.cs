using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerBehaviour : MonoBehaviour
{
    public Vector3 playerPos;
    public float maxHealth;
    public float health;
    public float armor;
    public Animator anim;
    public bool alive;
    public int currentSceneIndex;
    public bool teleUnlock;

    public bool canRegen;

    public GameObject heartController;
    public GameObject heartPrefab;
    public HeartController[] hearts;
    public HeartController[] heartLost;
    public HeartController[] heartSub;

    public bool healthStall;
    public float regenDelay;
    public float currentRegenDelay;
    public bool isMaxHealth;


    public bool outCave;

    public GameObject[] defaultTilemaps;
    public GameObject[] sideTilemaps;
    public GameObject defaultObjectArray;
    public GameObject sideObjectArray;
    public GameObject currentObjectArray;

    public GameObject close;
    public bool invincible;

    public AudioClip[] damageSounds;

    public GameObject soundSample;

    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        hearts = heartController.GetComponentsInChildren<HeartController>();
        currentObjectArray = defaultObjectArray;
        outCave = true;
        anim = GetComponent<Animator>();
        health = 3;
        alive = true;
        invincible = false;
        
    }


    // Update is called once per frame
    void Update()
    {
        currentSceneIndex = gameObject.scene.buildIndex;
        playerPos = transform.position;

        if (health <= 0 && alive)
        {
            Death();
        }

        if (alive)
        {
            if (healthStall && canRegen)
            {
                currentRegenDelay -= Time.deltaTime;

                if (currentRegenDelay <= 0)
                {
                    currentRegenDelay = regenDelay;

                    if (health < maxHealth)
                    {
                        isMaxHealth = false;
                        health++;
                        var heartPre = Instantiate(heartPrefab, transform.position, Quaternion.identity);
                        heartPre.transform.SetParent(heartController.transform);
                        hearts = heartController.GetComponentsInChildren<HeartController>();

                        if (health == maxHealth)
                        {
                            healthStall = false;
                            isMaxHealth = true;
                        }
                        
                        

                    }
                }
            }
        }

        if (active)
        {
            active = false;
            DeathByFalling();
        }
    }

    public void ChangeTile(bool def)
    {
        if (def)
        {
            outCave = true;
            currentObjectArray = defaultObjectArray;
            defaultObjectArray.SetActive(true);
            LeanTween.value(gameObject, 0f, 1f, 0.25f).setOnComplete(DisableSideTiles).setOnUpdate((float val) => {
                for(int i = 0; i < defaultTilemaps.Length; i++)
                {
                    defaultTilemaps[i].GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, val);
                }

                for(int i = 0; i < defaultObjectArray.transform.childCount; i++)
                {
                    if(defaultObjectArray.transform.GetChild(i).transform.childCount > 0 && defaultObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() == null)
                    {
                        for(int j = 0; j < defaultObjectArray.transform.GetChild(i).childCount; j++)
                        {
                            if(defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>() != null)
                            {
                                defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().color  = new Color(1f, 1f, 1f, val);
                            }

                            if (defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>() != null)
                            {
                                if (defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>() != null)
                                {
                                    defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>().originalIntensity * val;
                                }
                                else
                                {
                                    defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = val;
                                }


                                
                            }
                        }
                    }
                    else
                    {
                        if(defaultObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                        {
                            defaultObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, val);
                        }

                        if (defaultObjectArray.transform.GetChild(i).GetComponent<Light2D>() != null)
                        {
                            if (defaultObjectArray.transform.GetChild(i).GetComponent<LightOG>() != null)
                            {
                                defaultObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = defaultObjectArray.transform.GetChild(i).GetComponent<LightOG>().originalIntensity*val;
                            }
                            else
                            {
                                defaultObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = val;
                            }

                            
                        }

                    }

                }




                ///

                for (int i = 0; i < sideTilemaps.Length; i++)
                {
                    sideTilemaps[i].GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 1f-val);
                }


                for (int i = 0; i < sideObjectArray.transform.childCount; i++)
                {
                    if (sideObjectArray.transform.GetChild(i).transform.childCount > 0 && sideObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() == null)
                    {
                        for (int j = 0; j < sideObjectArray.transform.GetChild(i).childCount; j++)
                        {
                            if (sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>() != null)
                            {
                                sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - val);
                            }

                            if (sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>() != null)
                            {
                                if (sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>() != null)
                                {
                                    sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>().originalIntensity * (1f-val);
                                }
                                else
                                {
                                    sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = 1f-val;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (sideObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                        {
                            sideObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - val);
                        }

                        if (sideObjectArray.transform.GetChild(i).GetComponent<Light2D>() != null)
                        {
                            if (sideObjectArray.transform.GetChild(i).GetComponent<LightOG>() != null)
                            {
                                sideObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = sideObjectArray.transform.GetChild(i).GetComponent<LightOG>().originalIntensity * (1 - val);
                            }
                            else
                            {
                                sideObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = 1f - val;
                            }
                        }

                        
                    }

                }

            });
        }


        if (!def)
        {
            outCave = false;
            currentObjectArray = sideObjectArray;
            sideObjectArray.SetActive(true);
            LeanTween.value(gameObject, 0f, 1f, 0.25f).setOnComplete(DisableDefTiles).setOnUpdate((float val) => {
                for (int i = 0; i < defaultTilemaps.Length; i++)
                {
                    defaultTilemaps[i].GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 1f - val);
                }

                for (int i = 0; i < defaultObjectArray.transform.childCount; i++)
                {
                    if (defaultObjectArray.transform.GetChild(i).transform.childCount > 0 && defaultObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() == null)
                    {
                        for (int j = 0; j < defaultObjectArray.transform.GetChild(i).childCount; j++)
                        {
                            if (defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>() != null)
                            {
                                defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f-val);
                            }

                            if (defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>() != null)
                            {
                                if (defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>() != null)
                                {
                                    defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>().originalIntensity * (1f-val);
                                }
                                else
                                {
                                    defaultObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = 1f-val;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (defaultObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                        {
                            defaultObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f-val);
                        }

                        if (defaultObjectArray.transform.GetChild(i).GetComponent<Light2D>() != null)
                        {
                            if (defaultObjectArray.transform.GetChild(i).GetComponent<LightOG>() != null)
                            {
                                defaultObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = defaultObjectArray.transform.GetChild(i).GetComponent<LightOG>().originalIntensity * (1f-val);
                            }
                            else
                            {
                                defaultObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = 1f-val;
                            }
                        }
                    }

                }

                for (int i = 0; i < sideTilemaps.Length; i++)
                {
                    sideTilemaps[i].GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, val);
                }

                for (int i = 0; i < sideObjectArray.transform.childCount; i++)
                {
                    if (sideObjectArray.transform.GetChild(i).transform.childCount > 0 && sideObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() == null)
                    {
                        for (int j = 0; j < sideObjectArray.transform.GetChild(i).childCount; j++)
                        {
                            if (sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>() != null)
                            {
                                sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, val);
                            }

                            if (sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>() != null)
                            {
                                if (sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>() != null)
                                {
                                    sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<LightOG>().originalIntensity * val;
                                }
                                else
                                {
                                    sideObjectArray.transform.GetChild(i).GetChild(j).GetComponent<Light2D>().intensity = val;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (sideObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                        {
                            sideObjectArray.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, val);
                        }

                        if (sideObjectArray.transform.GetChild(i).GetComponent<Light2D>() != null)
                        {
                            if (sideObjectArray.transform.GetChild(i).GetComponent<LightOG>() != null)
                            {
                                sideObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = sideObjectArray.transform.GetChild(i).GetComponent<LightOG>().originalIntensity * val;
                            }
                            else
                            {
                                sideObjectArray.transform.GetChild(i).GetComponent<Light2D>().intensity = val;
                            }
                        }
                    }

                }

            });
        }
    }

    public void DisableDefTiles()
    {
        defaultObjectArray.SetActive(false);
        
        for (int i = 0; i < defaultTilemaps.Length; i++)
        {
            defaultTilemaps[i].GetComponent<TilemapCollider2D>().enabled = false;
        }

        for (int i = 0; i < sideTilemaps.Length; i++)
        {
            sideTilemaps[i].GetComponent<TilemapCollider2D>().enabled = true;
        }
    }

    public void DisableSideTiles()
    {

        
        sideObjectArray.SetActive(false);
        for (int i = 0; i < defaultTilemaps.Length; i++)
        {
            defaultTilemaps[i].GetComponent<TilemapCollider2D>().enabled = true;
        }

        for (int i = 0; i < sideTilemaps.Length; i++)
        {
            sideTilemaps[i].GetComponent<TilemapCollider2D>().enabled = false;
        }
    }

    

    public void TakeDamage(float damage)
    {
        if (alive && !invincible)
        {
            invincible = true;

            health -= damage;
            if(health < maxHealth)
            {
                isMaxHealth = false;
            }
            else
            {
                isMaxHealth = true;
            }

            GameObject.Find("Cam").GetComponent<CameraShake>().ShakeIt(0.04f, 0.025f);

            var sound = Instantiate(soundSample, transform.position, Quaternion.identity);

            AudioClip currentSound = damageSounds[(int)Random.Range(0, 2)];
            sound.GetComponent<SoundSample>().SpawnSound(currentSound, 0f, 1f);

            hearts = heartController.GetComponentsInChildren<HeartController>();


            heartSub = new HeartController[hearts.Length];
            heartLost = new HeartController[hearts.Length];

            if(health < 0)
            {
                for(int i = 0; i < hearts.Length; i++)
                {
                    if(hearts[i] != null)
                    {
                        hearts[i].Loss();
                    }
                }


            }
            else
            {
                for (int i = 0; i < hearts.Length - damage; i++)
                {
                    heartSub[i] = hearts[i];
                }

                for (int i = (int)(hearts.Length - damage); i < hearts.Length; i++)
                {
                    heartLost[i - (int)(hearts.Length - damage)] = hearts[i];
                }

                for (int i = 0; i < heartLost.Length; i++)
                {
                    if (heartLost[i] != null)
                    {
                        heartLost[i].Loss();
                    }
                }
                hearts = heartSub;
            }

            

            
            

            //hearts = heartController.GetComponentsInChildren<HeartController>();




            currentRegenDelay = regenDelay;
            healthStall = true;

            //ReassignHearts();

            GameObject.Find("LevelController").GetComponent<PauseMenu>().FreezeFrame(.075f);

            anim.Play("hit" + Random.Range(1, 3));
            Invoke("MakeVulnerable", 0.5f);
        }
        
    }

    public void ReassignHearts()
    {
        hearts = heartController.GetComponentsInChildren<HeartController>();
    }

    

    void Death()
    {
        GameObject.Find("LevelController").GetComponent<PauseMenu>().HideUI(true);
        GetComponent<PlayerController>().midTele = true;
        GameObject.Find("playerShadow").SetActive(false);
        GameObject.Find("RootShoot").GetComponent<Shoot>().currentGun.SetActive(false);
        alive = false;
        Invoke("DeathAnimation", 0.5f);
        Invoke("ReloadScene", 1.5f);
    }

    public void DeathByFalling()
    {
        if (!invincible)
        {
            GameObject.Find("LevelController").GetComponent<PauseMenu>().HideUI(true);
            alive = false;
            anim.Play("deathByFall");
            GetComponent<PlayerController>().midTele = true;
            GameObject.Find("playerShadow").SetActive(false);
            GameObject.Find("RootShoot").GetComponent<Shoot>().currentGun.SetActive(false);
            Invoke("ReloadScene", 1f);
        }

        
    }

    void ReloadScene()
    {

        PlayerData data = SaveSystem.LoadPlayer(GameObject.Find("InventoryManager").GetComponent<InventoryManager>().saveIndex);
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().saveIndex = 1;
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().loaded = true;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(scene);
        
        SceneManager.LoadScene(data.sceneIndex, LoadSceneMode.Additive);



        /*
        PlayerData data = SaveSystem.LoadPlayer(GameObject.Find("InventoryManager").GetComponent<InventoryManager>().saveIndex);
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().loaded = true;
        SceneManager.UnloadSceneAsync(GameObject.Find("InventoryManager").GetComponent<InventoryManager>().currentLevelSceneIndex);
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().loaded = true;
        SceneManager.LoadScene(data.sceneIndex, LoadSceneMode.Additive);
        LoadPlayer();
        */
    }

    void DeathAnimation()
    {
        anim.Play("deathAnim");
    }

    public int CheckCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void MakeVulnerable()
    {
        invincible = false;
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer(GameObject.Find("InventoryManager").GetComponent<InventoryManager>().saveIndex);

        //UnloadAllScenesExcept("ManagerScene");
        // SceneManager.LoadScene(data.sceneIndex);

        GameObject.Find("Player").GetComponent<PlayerBehaviour>().currentSceneIndex = data.sceneIndex;
        GameObject.Find("Player").GetComponent<PlayerBehaviour>().health = data.health;
        GameObject.Find("RootShoot").GetComponent<Shoot>().SwapGun(data.gunIndex, data.ammo);
        GameObject.Find("Player").GetComponent<DashAbility>().canDash = data.tele;
        Vector3 pos;
        pos.x = data.playerPosition[0];
        pos.y = data.playerPosition[1];
        pos.z = data.playerPosition[2];
        GameObject.Find("Player").transform.position = pos;

        Vector3 camPos;
        camPos.x = data.cameraPosition[0];
        camPos.y = data.cameraPosition[1];
        camPos.z = data.cameraPosition[2];
        GameObject.Find("Cam").transform.position = camPos;

        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().postcardInventory = data.postcardInventory;
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().scrap = data.scrap;
    }
}
