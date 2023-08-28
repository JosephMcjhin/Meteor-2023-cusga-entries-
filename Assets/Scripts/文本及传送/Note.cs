using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Inventory/New Note")]
public class Note : ScriptableObject
{
    public int note_id;
    [TextArea]
    public string[] text_con;
    public int[] tel_id;
    public int[] tel_loc;
    public int[] tel_st;
    public int[] tel_ed; 
}