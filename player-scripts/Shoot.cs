using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shoot : MonoBehaviour
{
    public Transform crosshair;
    public Transform player;
    public int gunType;

    public bool shootEnable;
    public bool swapEnable;

    public bool isMelee;

    public int currentGunIndex;
    public int currentWeaponIndex;

    //GUN SPECIFIC VARIABLES
    public GameObject currentGun;
    public GameObject currentBullet;
    public AudioClip currentSound;
    public float currentSoundVolume;
    public float currentGunShakeMag;
    public float currentGunShakeDuration;
    public float currentGunVolume;
    public float gundelay;
    public float bulletQuantity;
    public GameObject barrel;
    public float ShootSpeed;
    public float ammoCostPerShot;
    public float currentFreezeFrame;

    public bool canShoot;
    public float cooldownBase = 1f;
    public float cooldown = 0.25f;
    public float timeLeft = 0.0f;
    public Animator anim;
    public float animationLength;
    public float aniTimeLeft = 0.0f;

    public bool auto;

    public AudioSource audioSource;
    public GameObject soundSpawn;

    public GameObject reloadUI;
    public GameObject ammoText;
    public GameObject gunUI;

    //AMMO MANAGEMENT
    public float currentAmmo;
    public WeaponDescriptions weaponDesc;
    public float[] ammoStock;
    public GameObject[] interactableArray;



    //WEAPON LIST
    


    public GameObject[] weaponArray;

    //WEAPON UI ICONS
    public Sprite[] gunIcons;

    public float UITimer;
    float currentTime;
    public bool hidden;

    public float TimeTillShoot;

    // Start is called before the first frame update
    void Start()
    {
        isMelee = false;
        shootEnable = true;
        swapEnable = true;
        ammoText = GameObject.Find("AmmoText");
        reloadUI = GameObject.Find("ReloadCooldown");
        gunUI = GameObject.Find("GunUI");
        
        audioSource = GetComponent<AudioSource>();
        
        //SwapGun(-1,0);
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (shootEnable)
        {
            CheckShoot();
            ammoText.GetComponent<TextMeshProUGUI>().text = currentAmmo + "";
        }

        if (swapEnable)
        {
            if (Input.GetKeyDown("u"))
            {
                SwapGun(-1, 0);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isMelee)
                {
                    SwapGun(currentWeaponIndex, currentAmmo);
                }
                else
                {
                    SwapGun(7, 0);
                }

            }







            if (Input.GetKeyDown("1"))
            {
                SwapGun(1, 75);
            }
            if (Input.GetKeyDown("0"))
            {
                SwapGun(0, 30);
            }
            if (Input.GetKeyDown("2"))
            {
                SwapGun(2, 30);
            }

            if (Input.GetKeyDown("3"))
            {
                SwapGun(3, 30);
            }

            if (Input.GetKeyDown("4"))
            {
                SwapGun(4, 30);
            }

            if (Input.GetKeyDown("5"))
            {
                SwapGun(5, 30);
            }

            if (Input.GetKeyDown("6"))
            {
                SwapGun(6, 12);
            }

            if (Input.GetKeyDown("7"))
            {
                SwapGun(7, 0);
            }
        }
        

    }

    

    public void Fire()
    {
        //anim.SetBool("shoot", true);



        if(currentGunIndex != -1)
        {
            
            if(currentGunIndex == 7)
            {
                StartCoroutine(LateAim());

            }
            else
            {
                anim.Play("gunfire", 0, 0);
                
            }

            GameObject.Find("Cam").GetComponent<CameraShake>().ShakeIt(currentGunShakeMag, currentGunShakeDuration);



            for (int i = 0; i < bulletQuantity; i++)
            {
                Invoke("Spawn", gundelay);
            }


            if(currentSound != null)
            {
                var sound = Instantiate(soundSpawn, transform.position, Quaternion.identity);
                sound.GetComponent<SoundSample>().SpawnSound(currentSound, 0f, currentGunVolume);
            }
            
            //audioSource.Play();

            
        }
        
        // 0.01, 0.05
        

    }

    public IEnumerator LateAim()
    {

        
        currentGun.GetComponent<Aim>().aiming = false;
        currentGun.GetComponent<SwordSwing>().Swing();
        yield return new WaitForSeconds(0.25f);

        currentGun.GetComponent<Aim>().aiming = true;
    }

    public void Spawn()
    {
        if(currentGunIndex != 7)
        {
            Instantiate(currentBullet, barrel.transform.position, Quaternion.identity);
        }
        else
        {

            var swipe = Instantiate(currentBullet, barrel.transform.position, Quaternion.identity);
            swipe.GetComponent<Bullet>().UpOrDown(!currentGun.GetComponent<SwordSwing>().swingUp);
            

        }
        
    }


    public void Swings()
    {
        timeLeft = cooldownBase;
        if (currentGunIndex == 7)
        {
            StartCoroutine(GameObject.Find("Player").GetComponent<PlayerController>().SwingMove());
        }
        Fire();
    }

    void CheckShoot()
    {
        if(currentTime == 0 && hidden == false)
        {
            hidden = true;
            GameObject.Find("ReloadCooldown").GetComponent<ReloadAnimation>().HideTimer();
        }

        if(currentBullet != null)
        {
            if (auto)
            {
                if (Input.GetKey(KeyCode.Mouse0) && timeLeft <= 0 && currentAmmo >= ammoCostPerShot)
                {
                    currentAmmo -= ammoCostPerShot;
                    //ammoStock[currentGunIndex] -= ammoCostPerShot;
                    timeLeft = cooldownBase;
                    if(hidden != false && currentGunIndex != -1 && currentGunIndex != 7)
                    {
                        hidden = false;
                        GameObject.Find("ReloadCooldown").GetComponent<ReloadAnimation>().ShowTimer();
                    }

                    if (currentGunIndex == 7)
                    {
                        StartCoroutine(GameObject.Find("Player").GetComponent<PlayerController>().SwingMove());
                    }
                    currentTime = UITimer;
                    Fire();

                }
            }
            if (!auto)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && timeLeft <= 0 && currentAmmo >= ammoCostPerShot)
                {
                    currentAmmo -= ammoCostPerShot;
                    //ammoStock[currentGunIndex] -= ammoCostPerShot;
                    timeLeft = cooldownBase;
                    if (hidden != false && currentGunIndex != -1 && currentGunIndex != 7)
                    {
                        hidden = false;
                        GameObject.Find("ReloadCooldown").GetComponent<ReloadAnimation>().ShowTimer();
                    }
                    currentTime = UITimer;
                    Fire();

                }
            }
        }



        if (timeLeft > 0)
        {
            //e.gameObject.SetActive(false);
            canShoot = false;
            timeLeft -= Time.deltaTime;
        }
        else
        {
            //e.gameObject.SetActive(true);
            canShoot = true;
            timeLeft = 0;
            if(currentGunIndex == 7)
            {
                GameObject.Find("Player").GetComponent<PlayerController>().StartMove(0f);
            }
            
        }


        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0;
        }

    }

    public void SwapGun(int type, float ammo)
    {
        if (type == -1 || gunIcons[type] == null)
        {
            gunUI.GetComponent<Image>().sprite = null;
            gunUI.GetComponent<Image>().enabled = false;
        }

        else if (gunIcons[type] != null)
        {
            gunUI.GetComponent<Image>().enabled = true;
            gunUI.GetComponent<Image>().sprite = gunIcons[type];
        }
        
        //ammoStock[currentGunIndex] = currentAmmo;
        if(type != 7)
        {
            currentAmmo = ammo;
        }
        

        if (type == -1)
        {
            isMelee = false;
            currentGunIndex = -1;
            ammoCostPerShot = 0;
            currentWeaponIndex = -1;
            
            reloadUI.GetComponent<Image>().enabled = false;
            
            auto = true;
            currentGun.SetActive(false);
            weaponArray[0].SetActive(true);
            currentGun = weaponArray[0];
            currentBullet = null;

            currentSound = null;
            audioSource.clip = currentSound;
            audioSource.volume = 1f;

            currentFreezeFrame = 0f;
            anim = null;
            cooldownBase = 0.25f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }


        if (type == 0)
        {
            isMelee = false;
            currentGunIndex = 0;
            ammoCostPerShot = 1;
            currentWeaponIndex = 0;
            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;
            
            auto = true;
            currentGun.SetActive(false);
            weaponArray[1].SetActive(true);
            currentGun = weaponArray[1];
            currentBullet = (GameObject)Resources.Load("bullet0", typeof(GameObject));

            currentSound = null;
            audioSource.clip = currentSound;
            audioSource.volume = 1f;

            currentFreezeFrame = 0f;

            anim = currentGun.GetComponent<Animator>();
            cooldownBase = 0.25f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

        if (type == 1)
        {
            isMelee = false;
            currentGunIndex = 1;
            ammoCostPerShot = 1;
            currentWeaponIndex = 1;
            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;
            
            auto = true;
            currentGunShakeMag = 0.01f;
            currentGunShakeDuration = 0.05f;
            currentGun.SetActive(false);
            weaponArray[2].SetActive(true);
            currentGun = weaponArray[2];
            currentBullet = (GameObject)Resources.Load("bounceBullet", typeof(GameObject));

            currentSound = null;
            audioSource.clip = currentSound;
            currentGunVolume = 1f;

            anim = currentGun.GetComponent<Animator>();
            currentFreezeFrame = 0f;
            cooldownBase = 0.13f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

        if (type == 2)
        {
            isMelee = false;
            currentGunIndex = 2;
            ammoCostPerShot = 1;
            currentWeaponIndex = 2;
            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;
            
            auto = false;
            currentGunShakeMag = 0.04f;
            currentGunShakeDuration = 0.05f;
            currentGun.SetActive(false);
            weaponArray[3].SetActive(true);
            currentGun = weaponArray[3];
            currentBullet = (GameObject)Resources.Load("bullet2", typeof(GameObject));

            currentSound = null;
            audioSource.clip = currentSound;
            currentGunVolume = 1f;

            currentFreezeFrame = .25f;
            anim = currentGun.GetComponent<Animator>();
            cooldownBase = 1f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

        if (type == 3)
        {
            isMelee = false;
            currentGunIndex = 3;
            ammoCostPerShot = 3;
            //currentAmmo = ammoStock[3];
            currentWeaponIndex = 3;
            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;
            
            auto = false;
            currentGunShakeMag = 0.01f;
            currentGunShakeDuration = 0.05f;
            currentGun.SetActive(false);
            weaponArray[4].SetActive(true);
            currentGun = weaponArray[4];
            currentBullet = (GameObject)Resources.Load("bullet3", typeof(GameObject));

            currentSound = null;
            audioSource.clip = currentSound;
            currentGunVolume = 1f;

            anim = currentGun.GetComponent<Animator>();
            cooldownBase = 1.5f;
            gundelay = .3f;
            currentFreezeFrame = 0.08f;
            bulletQuantity = 4f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

        if (type == 4)
        {
            isMelee = false;
            currentGunIndex = 4;
            ammoCostPerShot = 1;
            currentWeaponIndex = 4;

            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;
            
            auto = true;
            currentGunShakeMag = 0.01f;
            currentGunShakeDuration = 0.05f;
            currentGun.SetActive(false);
            weaponArray[5].SetActive(true);
            currentGun = weaponArray[5];
            currentBullet = (GameObject)Resources.Load("rocketBullet", typeof(GameObject));

            currentSound = null;
            audioSource.clip = currentSound;
            currentGunVolume = 1f;

            anim = currentGun.GetComponent<Animator>();
            cooldownBase = .8f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

        if (type == 5)
        {
            isMelee = false;
            currentGunIndex = 5;
            ammoCostPerShot = 1;
            //currentAmmo = ammoStock[5];
            currentWeaponIndex = 5;
            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;
            
            auto = false;
            currentGunShakeMag = 0.01f;
            currentGunShakeDuration = 0.05f;
            currentGun.SetActive(false);
            weaponArray[6].SetActive(true);
            currentGun = weaponArray[6];
            currentBullet = (GameObject)Resources.Load("bullet5", typeof(GameObject));
            currentSound = (AudioClip)Resources.Load("GunSounds/sound5", typeof(AudioClip));
            audioSource.clip = currentSound;
            currentGunVolume = 1f;
            anim = currentGun.GetComponent<Animator>();
            currentFreezeFrame = 0.12f;
            cooldownBase = 1.2f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

        if (type == 6)
        {
            isMelee = false;
            currentGunIndex = 6;
            ammoCostPerShot = 1;
            //currentAmmo = ammoStock[6];
            currentWeaponIndex = 6;

            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;
            
            auto = false;

            currentGunShakeMag = 0.025f;
            currentGunShakeDuration = 0.02f;
            currentGun.SetActive(false);
            weaponArray[7].SetActive(true);
            currentGun = weaponArray[7];
            currentBullet = (GameObject)Resources.Load("bullet6", typeof(GameObject));
            anim = currentGun.GetComponent<Animator>();
            currentSound = (AudioClip)Resources.Load("GunSounds/revolver", typeof(AudioClip));
            audioSource.clip = currentSound;
            currentGunVolume = 0.25f;
            cooldownBase = 0.4f;

            currentFreezeFrame = 0.02f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

        if (type == 7)
        {
            currentGunIndex = 7;
            ammoCostPerShot = 0;
            //currentAmmo = ammoStock[6];

            isMelee = true;
            audioSource.Stop();
            reloadUI.GetComponent<Image>().enabled = true;

            auto = true;
            currentGunShakeMag = 0.03f;
            currentGunShakeDuration = 0.03f;
            currentGun.SetActive(false);
            weaponArray[8].SetActive(true);
            currentGun = weaponArray[8];
            currentBullet = (GameObject)Resources.Load("bullet7", typeof(GameObject));
            anim = currentGun.GetComponent<Animator>();
            currentSound = (AudioClip)Resources.Load("GunSounds/swordSwingGlitch", typeof(AudioClip));
            audioSource.clip = currentSound;
            currentGunVolume = 0.25f;
            cooldownBase = 0.6f;

            currentFreezeFrame = 0.05f;
            gundelay = 0f;
            bulletQuantity = 1f;
            barrel = currentGun.transform.GetChild(0).gameObject;

        }

    }
}
