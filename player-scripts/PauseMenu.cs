using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool paused;
    public bool inInventory;
    public GameObject pauseUI;
    public GameObject player;
    public GameObject crosshair;
    public GameObject rootShoot;
    public GameObject postcardGallery;
    public GameObject inventoryUI;

    public GameObject[] normalUI;

    public int selectedMain;
    public int selectedSlot;
    public bool discarding;

    public bool isFrozen;
    public bool canMenu;

    public GameObject useButton;

    public GameObject soundSample;
    public AudioClip[] UISounds;
    

    // Start is called before the first frame update
    void Start()
    {
        rootShoot = GameObject.Find("RootShoot");
        player = GameObject.Find("Player");
        crosshair = GameObject.Find("Crosshair");
        paused = false;

        canMenu = true;

        
        
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        inventoryUI.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canMenu)
        {
            if (!paused && !inInventory)
            {
                Pause();
                
            }
            else if (paused && !inInventory)
            {
                UnPause();
            }
            else if(!paused && inInventory)
            {
                CloseInventory();

            }
            



        }

        if (Input.GetKeyDown("q") && !paused && canMenu)
        {
            if (!inInventory)
            {
                OpenInventory();

            }
            else if (inInventory)
            {
                CloseInventory();
            }



        }

        if (Input.GetKeyDown("f") && !paused && !inInventory)
        {
            HotkeyActivate();



        }







    }


    public void FreezeFrame(float duration)
    {
        StartCoroutine(Freeze(duration));
    }

    private IEnumerator Freeze(float duration)
    {
        isFrozen = true;
        float ogTime = Time.timeScale;
        Time.timeScale = 0f;


        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        isFrozen = false;
    }

    public void DiscardToggle()
    {
        if (!discarding)
        {
            discarding = true;

        }
        else
        {
            discarding = false;
        }
    }

    void OpenInventory()
    {
        HideUI(true);
        crosshair.GetComponent<SpriteRenderer>().enabled = false;
        rootShoot.GetComponent<Shoot>().shootEnable = false;
        player.GetComponent<PlayerController>().interactEnable = false;
        inInventory = true;
        Time.timeScale = 0f;
        inventoryUI.SetActive(true);
        ResetGunDescription();
        inventoryUI.transform.GetChild(inventoryUI.transform.childCount - 4).GetComponent<TextMeshProUGUI>().text = GameObject.Find("InventoryManager").GetComponent<InventoryManager>().scrap + "";

        StartCoroutine(CheckSwap());
    }

    public void ResetGunDescription()
    {
        inventoryUI.transform.GetChild(inventoryUI.transform.childCount - 1).GetComponent<TextMeshProUGUI>().text = GetComponent<WeaponDescriptions>().WeaponDescriptionList(GameObject.Find("RootShoot").GetComponent<Shoot>().currentGunIndex);
        inventoryUI.transform.GetChild(inventoryUI.transform.childCount - 2).GetComponent<TextMeshProUGUI>().text = GetComponent<WeaponDescriptions>().WeaponNameList(GameObject.Find("RootShoot").GetComponent<Shoot>().currentGunIndex);
        inventoryUI.transform.GetChild(inventoryUI.transform.childCount - 3).GetComponent<Image>().sprite = GetComponent<WeaponDescriptions>().WeaponIcons(GameObject.Find("RootShoot").GetComponent<Shoot>().currentGunIndex);
    }

    public IEnumerator CheckSwap()
    {
        while (inInventory)
        {
            if(selectedMain != -1 && selectedSlot != -1)
            {
                SwapItems();
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }


    public void CloseInventory()
    {
        HideUI(false);
        crosshair.GetComponent<SpriteRenderer>().enabled = true;
        rootShoot.GetComponent<Shoot>().shootEnable = true;
        player.GetComponent<PlayerController>().interactEnable = true;
        inInventory = false;
        Time.timeScale = 1;
        if(selectedMain != -1)
        {
            GameObject.Find("Inventory").GetComponent<Inventory>().itemSlots[selectedMain].GetComponent<Image>().color = new Color(1, 1, 1, 1);

        }
        if(selectedSlot != -1)
        {
            GameObject.Find("Inventory").GetComponent<Inventory>().slotSlots[selectedSlot].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }


        GameObject.Find("Inventory").GetComponent<Inventory>().selectItem = -1;
        GameObject.Find("Inventory").GetComponent<Inventory>().selectSlot = -1;
        selectedMain = -1;
        selectedSlot = -1;
        GameObject.Find("Inventory").GetComponent<Inventory>().selectHot = -1;
        GameObject.Find("Inventory").GetComponent<Inventory>().hotSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        inventoryUI.SetActive(false);
        ResetUse();
        StopCoroutine(CheckSwap());
    }



    void Pause()
    {
        HideUI(true);
        crosshair.GetComponent<SpriteRenderer>().enabled = false;
        rootShoot.GetComponent<Shoot>().shootEnable = false;
        player.GetComponent<PlayerController>().interactEnable = false;
        paused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }


    public void UnPause()
    {
        HideUI(false);
        crosshair.GetComponent<SpriteRenderer>().enabled = true;
        rootShoot.GetComponent<Shoot>().shootEnable = true;
        player.GetComponent<PlayerController>().interactEnable = true;
        paused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    public void Gallery()
    {
        Instantiate(postcardGallery, new Vector3(GameObject.Find("UICanvas").transform.position.x, GameObject.Find("UICanvas").transform.position.y, GameObject.Find("UICanvas").transform.position.z), Quaternion.identity, GameObject.Find("UICanvas").transform);
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(GameObject.Find("InventoryManager").GetComponent<InventoryManager>(), GameObject.Find("Player").GetComponent<PlayerBehaviour>(), GameObject.Find("Cam").GetComponent<CamManager>(), GameObject.Find("RootShoot").GetComponent<Shoot>(), GameObject.Find("InventoryManager").GetComponent<InventoryManager>().saveIndex);
    }

    public void TestSave1()
    {
        SaveSystem.SavePlayer(GameObject.Find("InventoryManager").GetComponent<InventoryManager>(), GameObject.Find("Player").GetComponent<PlayerBehaviour>(), GameObject.Find("Cam").GetComponent<CamManager>(), GameObject.Find("RootShoot").GetComponent<Shoot>(), 1);
    }

    public void ReturnToMainMenu()
    {
        UnPause();
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1);
    }

    
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer(GameObject.Find("InventoryManager").GetComponent<InventoryManager>().saveIndex);

        //UnloadAllScenesExcept("ManagerScene");
        // SceneManager.LoadScene(data.sceneIndex);

        rootShoot.GetComponent<Shoot>().SwapGun(data.gunIndex, data.ammo);
        rootShoot.GetComponent<Shoot>().SwapGun(data.gunIndex, data.ammo);

        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().nameString = data.saveName;

        GameObject.Find("Player").GetComponent<PlayerBehaviour>().health = data.health;

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

        for(int i = 0; i < 20; i++)
        {
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().postcardInventory[i] = data.postcardInventory[i];
        }
        
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().scanInventory = data.scanInventory;
    }

    public void HideUI(bool yes)
    {
        if (yes)
        {
            for(int i = 0; i < normalUI.Length; i++)
            {
                normalUI[i].SetActive(false);
            }
        }

        if (!yes)
        {
            for (int i = 0; i < normalUI.Length; i++)
            {
                normalUI[i].SetActive(true);
            }
        }
    }

    public void SwapItems()
    {

        //&& (GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain].canEquip == true && GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot].canEquip == true)
        if (selectedSlot != -1 && selectedMain != -1 )
        {

            var sound = Instantiate(soundSample, transform.position, Quaternion.identity);
            sound.GetComponent<SoundSample>().SpawnSound(UISounds[0], 0f, 1);

            if (GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain] != null && GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot] != null)
            {
                if((GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain].canEquip == true && GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot].canEquip == true))
                {
                    Item stor = GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain];


                    GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain] = GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot];
                    GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot] = stor;
                }
                else
                {
                    var soundF = Instantiate(soundSample, transform.position, Quaternion.identity);
                    sound.GetComponent<SoundSample>().SpawnSound(UISounds[1], 0f, 1);
                }

                
            }

            else if (GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain] == null && GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot] != null)
            {

                if (GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot].canEquip == true)
                {
                    GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain] = GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot];
                    GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot] = null;
                }
                else
                {
                    var soundF = Instantiate(soundSample, transform.position, Quaternion.identity);
                    sound.GetComponent<SoundSample>().SpawnSound(UISounds[1], 0f, 1);
                }


            }

            else if (GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain] != null && GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot] == null)
            {
                if (GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain].canEquip == true)
                {
                    GameObject.Find("Inventory").GetComponent<Inventory>().slots[selectedSlot] = GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain];
                    GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain] = null;
                }
                else
                {
                    var soundF = Instantiate(soundSample, transform.position, Quaternion.identity);
                    sound.GetComponent<SoundSample>().SpawnSound(UISounds[1], 0f, 1);
                }



            }


            selectedSlot = -1;
            selectedMain = -1;

            GameObject.Find("Inventory").GetComponent<Inventory>().selectItem = -1;
            GameObject.Find("Inventory").GetComponent<Inventory>().selectSlot = -1;



            GameObject.Find("Inventory").GetComponent<Inventory>().RefreshUI();

        }

        ResetUse();

    }


    public bool CheckUse(int slotIndex)
    {
        
        if (GameObject.Find("Inventory").GetComponent<Inventory>().items[slotIndex] != null)
        {
            if(GameObject.Find("Inventory").GetComponent<Inventory>().items[slotIndex].canUse == true && GameObject.Find("Inventory").GetComponent<Inventory>().items[slotIndex].uses > 0)
            {
                print("canuse");
                useButton.SetActive(true);
                return true;

            }
            print("cannotuse");
            useButton.SetActive(false);
            return false;
        }
        print("empty");
        useButton.SetActive(false);
        return false;
    }

    public bool CheckEquipUse()
    {
        if (GameObject.Find("Inventory").GetComponent<Inventory>().hots[0] != null)
        {
            if (GameObject.Find("Inventory").GetComponent<Inventory>().hots[0].canUse == true && GameObject.Find("Inventory").GetComponent<Inventory>().hots[0].uses > 0)
            {
                print("canuse");
                useButton.SetActive(true);
                return true;

            }
            print("cannotuse");
            useButton.SetActive(false);
            return false;
        }
        print("empty");
        useButton.SetActive(false);
        return false;
    }

    public bool ResetUse()
    {
        useButton.SetActive(false);
        return false;
    }

    public void Use()
    {
        if (selectedMain != -1)
        {
            GameObject.Find("Inventory").transform.GetChild(0).GetComponent<InventoryStorage>().EffectFromUse(GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain].index);
            if (!GameObject.Find("Inventory").GetComponent<Inventory>().items[selectedMain].isCooldownBased)
            {
                GameObject.Find("Inventory").GetComponent<Inventory>().ClearSlot(selectedMain);
                ResetUse();
            }
            else
            {

            }




            selectedMain = -1;

            GameObject.Find("Inventory").GetComponent<Inventory>().selectItem = -1;
        }

        if(GameObject.Find("Inventory").GetComponent<Inventory>().selectHot != -1)
        {
            GameObject.Find("Inventory").transform.GetChild(0).GetComponent<InventoryStorage>().EffectFromUse(GameObject.Find("Inventory").GetComponent<Inventory>().hots[0].index);
            if (!GameObject.Find("Inventory").GetComponent<Inventory>().hots[0].isCooldownBased)
            {
                GameObject.Find("Inventory").GetComponent<Inventory>().ClearHotSlot(0);
                ResetUse();
            }
            else
            {

            }




            

            GameObject.Find("Inventory").GetComponent<Inventory>().selectHot = -1;
        }
        
        



        GameObject.Find("Inventory").GetComponent<Inventory>().RefreshUI();

    }

    public void HotkeyActivate()
    {
        if(GameObject.Find("Inventory").GetComponent<Inventory>().hots[0] != null)
        {
            GameObject.Find("Inventory").transform.GetChild(0).GetComponent<InventoryStorage>().EffectFromUse(GameObject.Find("Inventory").GetComponent<Inventory>().hots[0].index);
            if (!GameObject.Find("Inventory").GetComponent<Inventory>().hots[0].isCooldownBased)
            {
                GameObject.Find("Inventory").GetComponent<Inventory>().ClearHotSlot(0);
            }
            else
            {

            }

            
        }



        GameObject.Find("Inventory").GetComponent<Inventory>().RefreshUI();
    }



}

