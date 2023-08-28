using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Merge", menuName = "Inventory/New Merge")]
public class Merge : ScriptableObject{
    public List<string> recipe = new List<string>();
    public List<Item> product = new List<Item>();
}