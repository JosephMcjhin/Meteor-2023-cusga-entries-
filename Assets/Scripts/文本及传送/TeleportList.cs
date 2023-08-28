using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TeleportList", menuName = "Inventory/New TeleportList")]
public class TeleportList : ScriptableObject
{
    public bool[] unlocked;
    public List<Teleport> tel_list = new List<Teleport>();
}