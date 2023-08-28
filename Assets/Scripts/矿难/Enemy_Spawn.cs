using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawn : MonoBehaviour
{
    public GameObject enemy_prefab;
    public int enemy_level;
    public float spawn_time;
    float now_time;

    void Start(){
        now_time = 0;
    }

    void Update(){
        now_time -= Time.deltaTime;
        now_time = Mathf.Max(0f,now_time);
    }
    
    public void Enemy_spawn(){
        Instantiate(enemy_prefab, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other){
        Player a = other.gameObject.GetComponent<Player>();
        if(a != null && now_time <= 0){
            now_time = spawn_time;
            Enemy_spawn();
        }
    }
}
