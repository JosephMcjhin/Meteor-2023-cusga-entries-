using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> ItemList = new List<Item>();
    public List<int> NumList = new List<int>();
    public int max_capacity;
}