using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Spawn : MonoBehaviour
{
    public GameObject enemy_prefab;
    public int enemy_level;
    
    public void Enemy_spawn(){
        Instantiate(enemy_prefab, transform.position, Quaternion.identity);
    }

}
