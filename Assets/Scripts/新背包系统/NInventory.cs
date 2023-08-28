using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "NInventory/New Inventory")]
public class NInventory : ScriptableObject
{
    public List<NItem> ItemList = new List<NItem>();
    public List<int> NumList = new List<int>();
    public int max_capacity;
}