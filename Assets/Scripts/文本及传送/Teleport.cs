using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Teleport", menuName = "Inventory/New Teleport")]
public class Teleport : ScriptableObject
{
    public int tel_id;
    public string tel_time;
    public string tel_name;
    public string to_scene;
    public string to_sign;
    [TextArea]
    public string tel_info;
}