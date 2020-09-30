using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float intRange;
    public float useAmount;
    public float durability;
    public float ammo;

    public float index;

    public int itemIndex;

    public bool still;
    public bool unlimited;
    public float type;

    public bool canInteract;

    public bool clicked;

    public Animator anim;
    public GameObject animPrompt;
    public Animator promptAnim;

    public bool closest;
    
    public int scrapAmount;

    public Vector3 target;

    public float suctionRange;
    public float succStrength;

    public GameObject textPrompt;

    public float itemSpeed;
    public GameObject player;


    //TEXT VARIABLES
    
    
    public TextLine[] textLines;
    
    

    

    

    // Start is called before the first frame update
    void Start()
    {

        animPrompt = gameObject.transform.GetChild(0).gameObject;
        promptAnim = animPrompt.GetComponent<Animator>();
        animPrompt.GetComponent<SpriteRenderer>().enabled = false;
        useAmount = 0;
        GetComponent<SpriteRenderer>().enabled = true;
        player = GameObject.Find("Player");
        

        if(GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
        }

        if(type == 1)
        {
            anim = GetComponent<Animator>();
            anim.SetBool("Activated", false);
            anim.SetBool("Interacted", false);
        }

        if (type == 2)
        {
            
            transform.GetChild(0).right = transform.right;
            StartCoroutine(CheckPlayerSucc());
        }

        
    }

    void OnEnable()
    {
        if(type == 2)
        {
            StopAllCoroutines();
            StartCoroutine(CheckPlayerSucc());
        }
        
    }

    public IEnumerator CheckPlayerSucc()
    {
        while (useAmount == 0)
        {

            yield return new WaitForSecondsRealtime(0.016f);

            Vector3 offset = player.transform.position - transform.position;
            float sqrLen = offset.sqrMagnitude;

            if (sqrLen < intRange * intRange)
            {
                GiveScrap();
                
                useAmount++;
                if (!unlimited)
                {
                    durability--;
                }
                
                if (durability <= 0)
                {
                    canInteract = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                    player.GetComponent<PlayerBehaviour>().close = null;


                    Destroy(gameObject);
                    StopAllCoroutines();
                }


                
            }

            if (sqrLen < suctionRange * suctionRange)
            {
                float step = succStrength * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);

            }

            
        }

    }

    // Update is called once per frame
    void Update()
    {

        if(type == 2 && itemSpeed > 0.1f)
        {
            transform.position += target * itemSpeed * Time.deltaTime;
            itemSpeed = Mathf.Lerp(itemSpeed, 0f, 0.1f);
        }

        if (itemSpeed <= 0.1f && itemSpeed != -1 && itemSpeed != 0)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            GetComponent<Rigidbody2D>().freezeRotation = true;
            itemSpeed = -1;
        }

        

        if ((Vector2.Distance(player.transform.position, transform.position) < intRange) && canInteract == true && player.GetComponent<PlayerBehaviour>().close == gameObject)
        {
            animPrompt.GetComponent<SpriteRenderer>().enabled = true;
            promptAnim.SetBool("On", true);
        }
        else
        {
            if (promptAnim.GetBool("On") == true)
            {
                animPrompt.GetComponent<SpriteRenderer>().enabled = false;
                promptAnim.SetBool("On", false);
            }
        }

        if(canInteract == false && player.GetComponent<PlayerBehaviour>().close != gameObject)
        {
            animPrompt.GetComponent<SpriteRenderer>().enabled = false;
        }

        if ((Vector2.Distance(player.transform.position, transform.position) < intRange) && durability > 0 && canInteract && player.GetComponent<PlayerController>().interactEnable == true)
        {
            if (player.GetComponent<PlayerBehaviour>().close == null && player.GetComponent<PlayerBehaviour>().close != gameObject)
            {
                player.GetComponent<PlayerBehaviour>().close = gameObject;
            }

            if (player.GetComponent<PlayerBehaviour>().close != null && player.GetComponent<PlayerBehaviour>().close != gameObject)
            {
                if (Vector2.Distance(transform.position, player.transform.position) < Vector2.Distance(player.GetComponent<PlayerBehaviour>().close.transform.position, player.transform.position))
                {
                    player.GetComponent<PlayerBehaviour>().close = gameObject;
                }
            }


            
        }

        if((Vector2.Distance(player.transform.position, transform.position) > intRange) && player.GetComponent<PlayerBehaviour>().close == gameObject && durability > 0)
        {
            player.GetComponent<PlayerBehaviour>().close = null;
        }


        if ((Vector2.Distance(player.transform.position,transform.position) < intRange) && (Input.GetKeyDown("e")) && durability > 0 && canInteract && player.GetComponent<PlayerController>().interactEnable == true && player.GetComponent<PlayerBehaviour>().close == gameObject)
        {
            
            useAmount++;
            if (!unlimited)
            {
                durability--;
            }
            if(type == 0)
            {
                GunPickup();
            }
            if (type == 1 && useAmount == 1)
            {

                GetTele();
                
            }

            if (type == 1 && useAmount >= 2)
            {
                SpawnText();    
                anim.SetBool("Activated", false);
                anim.SetBool("Interacted", true);
                canInteract = false;
                player.GetComponent<DashAbility>().canDash = true;
                GameObject.Find("InventoryManager").GetComponent<InventoryManager>().canDash = true;



            }

            //scrap
            if(type == 2)
            {
                GiveScrap();
            }

            if(type == 3)
            {
                SpawnText();
            }

            if(type == 4)
            {
                anim.Play("saveMachine");
                SpawnText();
                
            }
            if(type == 5)
            {
                SpawnText();
            }

            if(type == 6)
            {
                GameObject.Find("Inventory").GetComponent<Inventory>().PickUp(itemIndex);
            }

            if(type == 7)
            {
                GameObject.Find("Inventory").GetComponent<Inventory>().PickUp(itemIndex);
                SpawnText();
            }
            
        }

        if(durability <= 0)
        {
            canInteract = false;
            GetComponent<SpriteRenderer>().enabled = false;
            player.GetComponent<PlayerBehaviour>().close = null;
            StopAllCoroutines();
            Destroy(gameObject);


        }

        if (clicked)
        {
            clicked = false;
            
        }

        
    }

    void GiveScrap()
    {
        if(GameObject.Find("InventoryManager") != null)
        {
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().scrap += scrapAmount;
        }
    }

    void GunPickup()
    {
        if(GameObject.Find("RootShoot").GetComponent<Shoot>().currentGunIndex != -1 && GameObject.Find("RootShoot").GetComponent<Shoot>().currentGunIndex != 7)
        {
            GameObject summon = (GameObject)Instantiate(GameObject.Find("RootShoot").GetComponent<Shoot>().interactableArray[GameObject.Find("RootShoot").GetComponent<Shoot>().currentGunIndex], new Vector3(player.transform.position.x, player.transform.position.y-0.5f, player.transform.position.z), Quaternion.identity);
            summon.transform.parent = GameObject.Find("Player").GetComponent<PlayerBehaviour>().currentObjectArray.transform;
            summon.GetComponent<Interactable>().ammo = GameObject.Find("RootShoot").GetComponent<Shoot>().currentAmmo;
        }
        
        GameObject.Find("RootShoot").GetComponent<Shoot>().SwapGun((int)index, ammo);
    }
    
    void GetTele()
    {
        anim.SetBool("Activated",true);
        
        Invoke("CanActivate",3f);
        canInteract = false;


    }

    void CanActivate()
    {
        
        canInteract = true;
        animPrompt.transform.localPosition = new Vector3(animPrompt.transform.localPosition.x, animPrompt.transform.localPosition.y + 0.5f, animPrompt.transform.localPosition.z);
        



    }

    void SpawnText()
    {
        var text = Instantiate(textPrompt, new Vector3(GameObject.Find("UICanvas").transform.position.x, GameObject.Find("UICanvas").transform.position.y, GameObject.Find("UICanvas").transform.position.z), Quaternion.identity, GameObject.Find("UICanvas").transform);
        if(text.GetComponent<textboxSave>() != null)
        {
            text.GetComponent<textboxSave>().tiedInteractable = gameObject;
        }

        if (text.GetComponent<TextboxFull>() != null)
        {
            text.GetComponent<TextboxFull>().tiedInteractable = gameObject;
        }

        if (text.GetComponent<NotificationScript>() != null)
        {
            text.GetComponent<NotificationScript>().tiedInteractable = gameObject;
        }

        if (text.GetComponent<textboxStandard>() != null)
        {
            text.GetComponent<textboxStandard>().tiedInteractable = gameObject;
        }

        if (text.GetComponent<textboxTeleporter>() != null)
        {
            text.GetComponent<textboxTeleporter>().tiedInteractable = gameObject;
        }

        /*
        if (text.GetComponent<TextBoxManager>() != null)
        {
            text.GetComponent<TextBoxManager>().textLines = textLines;
            text.GetComponent<TextBoxManager>().tiedInteractable = gameObject;
        }

        if (text.GetComponent<TextBoxChoice>() != null)
        {
            text.GetComponent<TextBoxChoice>().textLines = textLines;
            text.GetComponent<TextBoxChoice>().tiedInteractable = gameObject;
        }
        */


    }

    

    


}
