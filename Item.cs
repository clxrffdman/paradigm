
using UnityEngine;


[CreateAssetMenu]
public class Item : ScriptableObject
{
    //Item is a scriptable inventory object. 
    public string ItemName;
    public Sprite Icon;
    public int index;
    public bool canEquip;
    public bool canUse;
    public int uses;
    public bool isCooldownBased;
    public float cooldown;
    public string description;
}
