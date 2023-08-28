using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Combat/New Wave")]
public class EnemyWave : ScriptableObject
{
    public List<GameObject> enemy_list = new List<GameObject>();
    public List<int> pos_list = new List<int>();
}