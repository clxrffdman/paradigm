using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropClick : MonoBehaviour, IPointerClickHandler
{
    public PauseMenu pauseMenu;
    public Inventory inventory;
    public int slotIndex;
    public bool isSlot;
    public bool isHot;

    public void OnPointerClick(PointerEventData eventData)
    {
        //print("clicked");

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (pauseMenu.discarding == true)
            {
                print("DiscaRD");
                inventory.Drop(slotIndex);
            }
            else
            {
                
                if(!isSlot && !isHot && inventory.items[slotIndex] != null && inventory.items[slotIndex].canUse == true)
                {
                    if (inventory.hots[0] == null)
                    {
                        inventory.hots[0] = inventory.items[slotIndex];
                        inventory.items[slotIndex] = null;

                        var sound = Instantiate(pauseMenu.soundSample, transform.position, Quaternion.identity);
                        sound.GetComponent<SoundSample>().SpawnSound(pauseMenu.UISounds[0], 0f, 1);

                        pauseMenu.selectedSlot = -1;
                        inventory.selectSlot = -1;
                        pauseMenu.selectedMain = -1;
                        inventory.selectItem = -1;
                        inventory.selectHot = -1;


                        inventory.hotSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    }
                    else
                    {
                        Item stor = inventory.hots[0];
                        inventory.hots[0] = inventory.items[slotIndex];
                        inventory.items[slotIndex] = stor;

                        var sound = Instantiate(pauseMenu.soundSample, transform.position, Quaternion.identity);
                        sound.GetComponent<SoundSample>().SpawnSound(pauseMenu.UISounds[0], 0f, 1);

                        pauseMenu.selectedSlot = -1;
                        inventory.selectSlot = -1;
                        pauseMenu.selectedMain = -1;
                        inventory.selectItem = -1;
                        inventory.selectHot = -1;
                    }

                    inventory.RefreshUI();
                }

                if (!isSlot && isHot && inventory.hots[0] != null)
                {
                    inventory.PickUp(inventory.hots[0].index);
                    inventory.hots[0] = null;

                    var sound = Instantiate(pauseMenu.soundSample, transform.position, Quaternion.identity);
                    sound.GetComponent<SoundSample>().SpawnSound(pauseMenu.UISounds[0], 0f, 1);

                    pauseMenu.selectedSlot = -1;
                    inventory.selectSlot = -1;
                    pauseMenu.selectedMain = -1;
                    inventory.selectItem = -1;
                    inventory.selectHot = -1;
                    pauseMenu.ResetGunDescription();

                    inventory.hotSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    
                    inventory.RefreshUI();
                }



            }
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {


            print("LeftClicked");

            if (!isSlot && !isHot)
            {
                inventory.hotSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);

                if (pauseMenu.selectedMain != slotIndex)
                {
                    if (inventory.items[slotIndex] != null && pauseMenu.selectedSlot == -1)
                    {
                        inventory.RewriteItemDescription(inventory.items[slotIndex].index);
                    }

                    pauseMenu.selectedMain = slotIndex;
                    inventory.selectItem = slotIndex;
                    inventory.selectHot = -1;
                    pauseMenu.CheckUse(slotIndex);
                }
                else
                {
                    pauseMenu.ResetGunDescription();
                    pauseMenu.selectedMain = -1;
                    inventory.selectItem = -1;
                    inventory.selectHot = -1;
                    pauseMenu.ResetUse();
                }

                
            }

            

            if (isSlot)
            {
                inventory.hotSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);

                if (pauseMenu.selectedSlot != slotIndex)
                {
                    if(inventory.slots[slotIndex] != null && pauseMenu.selectedMain == -1)
                    {
                        inventory.RewriteItemDescription(inventory.slots[slotIndex].index);
                    }
                    
                    pauseMenu.selectedSlot = slotIndex;
                    inventory.selectSlot = slotIndex;
                    inventory.selectHot = -1;
                    pauseMenu.ResetUse();
                    //pauseMenu.CheckUse(slotIndex);
                }
                else
                {
                    pauseMenu.ResetGunDescription();
                    pauseMenu.selectedSlot = -1;
                    inventory.selectSlot = -1;
                    inventory.selectHot = -1;
                    pauseMenu.ResetUse();
                }
                
            }

            if (isHot && inventory.hots[0] != null)
            {
                if(inventory.selectHot == 0)
                {
                    inventory.selectHot = -1;
                    inventory.hotSlots[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    pauseMenu.ResetGunDescription();
                }
                else
                {
                    pauseMenu.selectedSlot = -1;
                    inventory.selectSlot = -1;
                    pauseMenu.selectedMain = -1;
                    inventory.selectItem = -1;


                    
                    pauseMenu.ResetUse();

                    inventory.selectHot = 0;

                    inventory.hotSlots[0].GetComponent<Image>().color = Color.red;
                    pauseMenu.CheckEquipUse();
                    inventory.RewriteItemDescription(inventory.hots[slotIndex].index);

                }

                
            }

            inventory.RefreshUI();
        }



    }



}
