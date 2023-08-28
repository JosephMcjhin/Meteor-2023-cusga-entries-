using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite ItemImage;
    [TextArea]
    public string ItemInfo;
    public GameObject Weapon;
    public string ItemID;
}
