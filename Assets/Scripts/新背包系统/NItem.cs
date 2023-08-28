using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "NInventory/New Item")]
public class NItem : ScriptableObject
{
    public string ItemID;
    public string ItemName;
    public Sprite ItemImage;
    [TextArea]
    public string ItemInfo;
    public GameObject Weapon;
    public int ItemClass;
}