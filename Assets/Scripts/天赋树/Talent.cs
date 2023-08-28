using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Talent", menuName = "Talent/New Talent")]
public class Talent : ScriptableObject
{
    public List<int> talent_level = new List<int>();
    public List<int> max_level = new List<int>();
    [TextArea]
    public string[] talent_info = new string[7];
    public int talent_point;
}