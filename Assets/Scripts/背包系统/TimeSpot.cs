using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TimeSpot", menuName = "Inventory/New TimeSpot")]
public class TimeSpot : ScriptableObject
{
    public List<int> spot_list = new List<int>();
    public List<bool> is_lock = new List<bool>();
}
