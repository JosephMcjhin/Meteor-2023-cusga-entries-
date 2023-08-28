using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatTrigger : MonoBehaviour
{
    public int enemy_id;
    public string to_scene;
    public List<EnemyWave> wave_list = new List<EnemyWave>();

    void OnTriggerEnter2D(Collider2D other){
        Player a = other.gameObject.GetComponent<Player>();
        if(a != null){
            CombatManager.instance.wave_list = wave_list;
            CombatManager.instance.pre_scene = SceneManager.GetActiveScene().name;
            CombatManager.instance.enemy_dead[enemy_id] = true;
            CombatManager.instance.Combat_start(to_scene);
        }
    }
}
