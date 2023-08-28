using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Inventory/New State")]
public class PlayerState : ScriptableObject
{
    //第一项为是否救竹林隐士
    public bool[] state_list;
}