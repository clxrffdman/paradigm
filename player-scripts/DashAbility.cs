using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DashAbility : MonoBehaviour
{
    public Shoot shoot;

    public float dashCooldown;
    public float cooldown;
    public float timeLeft = 0.0f;
    public bool canDash;

    public GameObject teleFX;
    public Animator teleAnim;
    public GameObject player;
    public GameObject telelocation;
    public GameObject rootShoot;

    public Vector3 oldPos;

    public bool isTracking;
    public float oldTime;

    public GameObject teleCooldownSprite;
    public GameObject ammoTextSprite;
    public Animator cooldownUIAnim;

    public GameObject reloadController;

    public GameObject companion;

    public GameObject swordDeath;
    public GameObject damageNumber;

    //DASH ATTACK INSTANCE VARIABLES
    public bool isDashAttack;
    public DashAttack dashAttack;
    public float dashDuration;
    //public RaycastHit2D attackRay;
    //public LayerMask dashAttackTargets;
    public int dashAttackDamage;
    //public float dashAttackRadius;



    public bool a;

    // Start is called before the first frame update
    void Start()
    {
        telelocation = GameObject.Find("TeleLocation");
        teleCooldownSprite = GameObject.Find("TeleCooldown");
        teleCooldownSprite.GetComponent<Image>().enabled = false;
        cooldownUIAnim = teleCooldownSprite.GetComponent<Animator>();
        isTracking = true;
        canDash = false;
        GetComponent<SpriteRenderer>().enabled = true;
        reloadController = GameObject.Find("ReloadCooldown");
        rootShoot = GameObject.Find("RootShoot");

        player = GameObject.Find("Player");
        teleFX = GameObject.Find("teleportFX");
        teleAnim = teleFX.GetComponent<Animator>();

        teleAnim.SetBool("TeleTime", false);
        teleFX.GetComponent<SpriteRenderer>().enabled = false;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            canDash = true;
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().canDash = true;
        }

        if (isTracking)
        {
            teleFX.transform.position = player.transform.position;
        }

        if(teleAnim.GetBool("TeleTime") == true)
        {
            teleAnim.SetBool("TeleTime", false);
        }

        

        if (canDash)
        {
            if(teleCooldownSprite.GetComponent<Image>().enabled == false)
            {
                teleCooldownSprite.GetComponent<Image>().enabled = true;
                ammoTextSprite.GetComponent<RectTransform>().localPosition = new Vector3(-272.7f, 63, 0);
                GameObject.Find("InventoryManager").GetComponent<InventoryManager>().canDash = true;
            }

            GetComponent<PlayerBehaviour>().teleUnlock = true;

            thrustApply();



        }
        
    }

    void thrustApply()
    {
        timeLeft -= Time.deltaTime;

        

        if (Input.GetKey(KeyCode.LeftShift) && timeLeft <= 0)
        {
            


            telelocation.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            telelocation.transform.GetChild(0).right = telelocation.transform.position - player.transform.position;

            telelocation.transform.GetChild(0).GetComponent<LineRenderer>().enabled = true;

            if (rootShoot.GetComponent<Shoot>().currentGunIndex == 7 && Input.GetKeyDown(KeyCode.Mouse0))
            {

                dashAttack.checkingAttackAngle = false;
                telelocation.transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
                telelocation.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                timeLeft = cooldown;
                isTracking = true;
                cooldownUIAnim.SetBool("OnCooldown", true);
                oldPos = telelocation.transform.position;
                telelocation.GetComponent<AudioSource>().Play();
                GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("RootShoot").GetComponent<Shoot>().currentGun.GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("RootShoot").GetComponent<Shoot>().shootEnable = false;
                teleFX.GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<PlayerController>().midTele = true;
                teleAnim.SetBool("TeleTime", true);
                isDashAttack = true;
                Invoke("Dash", 0.2f);
            }

        }


        if(Input.GetKey(KeyCode.LeftShift) && timeLeft <= 0 && shoot.currentGunIndex == 7)
        {
            shoot.shootEnable = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shoot.shootEnable = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && timeLeft <= 0)
        {
            


            shoot.shootEnable = false;
            
            telelocation.transform.GetChild(0).GetComponent<LineRenderer>().enabled = false;
            telelocation.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            timeLeft = cooldown;
            isTracking = true;
            cooldownUIAnim.SetBool("OnCooldown",true);
            oldPos = telelocation.transform.position;
            telelocation.GetComponent<AudioSource>().Play();
            GetComponent<SpriteRenderer>().enabled = false;
            shoot.GetComponent<Shoot>().currentGun.GetComponent<SpriteRenderer>().enabled = false;
            shoot.GetComponent<Shoot>().shootEnable = false;
            teleFX.GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<PlayerController>().midTele = true;
            teleAnim.SetBool("TeleTime", true);
            isDashAttack = false;
            Invoke("Dash", 0.2f);
        }

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0;
            
        }


    }

    public IEnumerator PlayerInvincible()
    {
        
        player.GetComponent<PlayerBehaviour>().invincible = true;

        yield return new WaitForSeconds(dashDuration+ 0.05f);

        

        player.GetComponent<PlayerBehaviour>().invincible = false;


        dashAttack.checkingAttackAngle = true;

    }

    void Dash()
    {
        if (isDashAttack)
        {
            LeanTween.move(gameObject, oldPos, dashDuration);
            StopCoroutine(PlayerInvincible());
            StartCoroutine(PlayerInvincible());
        }
        else
        {
            GetComponent<PlayerController>().rb.position = oldPos;
        }

        

        GetComponent<SpriteRenderer>().enabled = true;
        shoot.currentGun.GetComponent<SpriteRenderer>().enabled = true;

        if (isDashAttack)
        {
            
            DashSwipe();
            
        }

        if (shoot.currentGunIndex != 7)
        {
            shoot.shootEnable = true;
        }
        
        oldTime = shoot.timeLeft;
        

        if(shoot.timeLeft != 0 && shoot.auto == false)
        {
            shoot.timeLeft = 0;
            reloadController.GetComponent<ReloadAnimation>().enabled = false;
            reloadController.GetComponent<Animator>().Play("reload12");
            Invoke("EndSpecial", 0.15f);
            Invoke("ReRate", 0.3f);
        }

        

        isTracking = false;
        Invoke("TeleInsta", 0.1f);
        GetComponent<PlayerController>().midTele = false;
        Invoke("DisableSprite",0.5f);
    }

    void DisableSprite()
    {
        teleFX.GetComponent<SpriteRenderer>().enabled = false;

        
    }

    void TeleInsta()
    {
        if (companion != null)
        {
            companion.GetComponent<CompanionController>().TeleLocate();
        }

        teleFX.transform.position = player.transform.position;

        if (isDashAttack)
        {
            teleFX.transform.position = player.transform.position;

            shoot.Swings();
            isDashAttack = false;
        }
        shoot.shootEnable = true;



        cooldownUIAnim.SetBool("OnCooldown", false);
    }

    void ReRate()
    {
        a = true;

        shoot.timeLeft = oldTime;
        reloadController.GetComponent<ReloadAnimation>().enabled = true;

    }

    void EndSpecial()
    {
        reloadController.GetComponent<Animator>().Play("reload12");
    }

    public void DashSwipe()
    {
        

        foreach (Collider2D other in dashAttack.colliders)
        {
            


            if (other.gameObject.tag == "Wall")
            {
                if (other.gameObject.GetComponent<wall>() != null)
                {
                    other.gameObject.GetComponent<wall>().TakeDamage(dashAttackDamage);

                }
            }

            if (other.gameObject.tag == "Destruct")
            {
                other.gameObject.GetComponent<DestRock>().TakeDamage(dashAttackDamage, true);
            }

            if (other.gameObject.tag == "Enemy")
            {
                Instantiate(swordDeath, other.gameObject.transform.position, Quaternion.identity);

                var dmgNumber = Instantiate(damageNumber, transform.position, Quaternion.identity);
                dmgNumber.GetComponent<BulletNumber>().SetVal(dashAttackDamage);
                if (GetComponent<Rigidbody2D>().velocity.x > 0)
                {
                    dmgNumber.GetComponent<BulletNumber>().right = false;
                }
                else
                {
                    dmgNumber.GetComponent<BulletNumber>().right = true;
                }



                if (other.gameObject.GetComponent<EnemyBehaviour>() != null)
                {
                    other.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(dashAttackDamage, 0);
                }

                if (other.gameObject.GetComponent<MeleeEnemyBehaviour>() != null)
                {
                    other.gameObject.GetComponent<MeleeEnemyBehaviour>().TakeDamage(dashAttackDamage, 0);
                }

                if (other.gameObject.GetComponent<ClapEnemyBehaviour>() != null)
                {
                    other.gameObject.GetComponent<ClapEnemyBehaviour>().TakeDamage(dashAttackDamage, 0);
                }
            }

            }

        //attackRay = Physics2D.CircleCast(transform.position, dashAttackRadius, telelocation.transform.position);
    }

}
