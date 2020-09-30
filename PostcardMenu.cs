using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostcardMenu : MonoBehaviour
{
    public GameObject[] postcards;
    public bool active;
    


    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < 12; i++)
        {
            if(GameObject.Find("InventoryManager").GetComponent<InventoryManager>().postcardInventory[i] == true)
            {
                postcards[i].GetComponent<PostcardWarp>().unlocked = true;
            }
            
        }
        
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 0f).setIgnoreTimeScale(true);
        OnOpen();
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnOpen()
    {
        LeanTween.scale(gameObject, new Vector3(7f, 7f, 7f), 0.5f).setIgnoreTimeScale(true).setOnComplete(SetActive);
        GameObject.Find("Player").GetComponent<PlayerController>().midTele = true;
        
        GameObject.Find("RootShoot").GetComponent<Shoot>().shootEnable = false;
    }

    public void OnClose()
    {
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 0.5f).setIgnoreTimeScale(true).setOnComplete(DestroyMe);
        GameObject.Find("Player").GetComponent<PlayerController>().midTele = false;
        GameObject.Find("RootShoot").GetComponent<Shoot>().shootEnable = true;
        
    }

    void DestroyMe()
    {
        //tiedInteractable.GetComponent<Interactable>().canInteract = true;
        Destroy(gameObject);
    }

    void SetActive()
    {
        active = true;
    }

    
}
