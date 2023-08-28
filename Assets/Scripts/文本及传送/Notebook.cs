using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Notebook", menuName = "Inventory/New Notebook")]
public class Notebook : ScriptableObject
{
    public bool[] is_added;
    public List<Note> note = new List<Note>();
}