using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    //SLOTINDEX OF SELECTED ITEM/SLOT
    public int selectItem;
    public int selectSlot;
    public int selectHot;

    //ITEM ARRAYS
    public List<Item> items;
    public List<Item> slots;
    public List<Item> hots;

    //ITEMSLOT PARENTS
    [SerializeField] Transform itemsParent;
    [SerializeField] Transform slotsParent;
    [SerializeField] Transform hotParent;

    //ITEMSLOT OBJECT ARRAYS
    public ItemSlot[] itemSlots;
    public ItemSlot[] slotSlots;
    public ItemSlot[] hotSlots;

    public Image uiHotspot;
    public InventoryStorage inventoryStorage;

    //IN-INVENTORY ITEM TEXT ELEMENTS
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image itemImage;

    void Start()
    {
        if(inventoryStorage == null)
        {
            inventoryStorage = transform.GetChild(0).GetComponent<InventoryStorage>();
        }
    }


    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }

        if (slotsParent != null)
        {
            slotSlots = slotsParent.GetComponentsInChildren<ItemSlot>();
        }

        RefreshUI();
    }



    public void RefreshUI()
    {
        /*
        for (int z = 0; z < itemSlots.Length; z++)
        {
            
            if(items[z] != null)
            {
                if (items[z].uses == 0)
                {
                    items[z] = null;
                }
            }
            
        }

        for (int z = 0; z < slotSlots.Length; z++)
        {
            if(slots[z] != null)
            {
                if (slots[z].uses == 0)
                {
                    slots[z] = null;
                }
            }
            
        }
        */



        int i = 0;
        for (; i < items.Count && i < itemSlots.Length; i++)
        {

            itemSlots[i].Item = items[i];
            itemSlots[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }



        int j = 0;
        for (; j < slots.Count && j < slotSlots.Length; j++)
        {
            slotSlots[j].Item = slots[j];
            slotSlots[j].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        for (; j < slotSlots.Length; j++)
        {
            slotSlots[j].Item = null;
        }

        int k = 0;
        for (; k < hots.Count && k < hotSlots.Length; k++)
        {
            hotSlots[k].Item = hots[k];
            
                //hotSlots[k].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            
            
        }

        for (; k < hotSlots.Length; k++)
        {
            hotSlots[k].Item = null;
        }


        //&& itemSlots[selectItem].GetComponent<Image>().sprite != null
        if (selectItem != -1 )
        {
            itemSlots[selectItem].GetComponent<Image>().color = Color.red;
        }

        //&& slotSlots[selectSlot].GetComponent<Image>().sprite != null
        if (selectSlot != -1 )
        {
            slotSlots[selectSlot].GetComponent<Image>().color = Color.red;
        }

        if(hots[0] != null)
        {
            uiHotspot.enabled = true;
            uiHotspot.sprite = hots[0].Icon;
        }
        else
        {
            uiHotspot.enabled = false;
        }
        

        if (GameObject.Find("InventoryManager") != null)
        {
            if (GameObject.Find("InventoryManager").GetComponent<InventoryManager>() != null)
            {
                AssignToManager();
            }
        }
        


    }

    public void RewriteItemDescription(int index)
    {
        itemDescription.text = inventoryStorage.descriptionFromIndex(index);
        itemImage.sprite = inventoryStorage.itemList[index].Icon;
        itemName.text = inventoryStorage.itemList[index].ItemName;

    }

    public void AssignToManager()
    {
        if (GameObject.Find("InventoryManager") != null)
        {
            for (int j = 0; j < items.Count; j++)
            {
                if(items[j] == null)
                {
                    GameObject.Find("InventoryManager").GetComponent<InventoryManager>().inventory[j] = -1;
                }
                else
                {
                    GameObject.Find("InventoryManager").GetComponent<InventoryManager>().inventory[j] = items[j].index;
                }

                
            }


            for (int j = 0; j < slots.Count; j++)
            {
                if (slots[j] == null)
                {
                    GameObject.Find("InventoryManager").GetComponent<InventoryManager>().slotInventory[j] = -1;
                }
                else
                {
                    GameObject.Find("InventoryManager").GetComponent<InventoryManager>().slotInventory[j] = slots[j].index;
                }


            }

            for (int j = 0; j < hots.Count; j++)
            {
                if (hots[j] == null)
                {
                    GameObject.Find("InventoryManager").GetComponent<InventoryManager>().hotInventory[j] = -1;
                }
                else
                {
                    GameObject.Find("InventoryManager").GetComponent<InventoryManager>().hotInventory[j] = hots[j].index;
                }


            }
        }
    }




    public void PickUp(int index)
    {
        int end = -1;
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i] == null)
            {
                end = i;
                i = items.Count;
            }
        }

        if(end != -1)
        {
            items[end] = transform.GetChild(0).GetComponent<InventoryStorage>().itemList[index];
        }
        else
        {
            print("Error: Inventory is full");
        }

        RefreshUI();
        AssignToManager();

    }

    public void Drop(int slotIndex)
    {
        items[slotIndex] = null;

        RefreshUI();
        AssignToManager();
    }

    public void ClearSlot(int slotIndex)
    {
        items[slotIndex] = null;

        RefreshUI();
        AssignToManager();
    }

    public void ClearHotSlot(int slotIndex)
    {
        hots[slotIndex] = null;

        RefreshUI();
        AssignToManager();
    }



}
