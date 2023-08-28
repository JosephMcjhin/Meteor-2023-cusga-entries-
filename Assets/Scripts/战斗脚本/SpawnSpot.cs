using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpot : MonoBehaviour
{
    public List<Transform> spot_list = new List<Transform>();
    public Transform player_pos;
    List<EnemyWave> wave_list = new List<EnemyWave>();
    int now_wave;

    IEnumerator Enemy_gen(){
        CombatManager.instance.enemy_num = wave_list[now_wave].enemy_list.Count;
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < CombatManager.instance.enemy_num; i ++){
            Instantiate(wave_list[now_wave].enemy_list[i], spot_list[wave_list[now_wave].pos_list[i]].position, Quaternion.identity);
        }
        now_wave ++;
        yield return null;
    }

    void Start(){
        Player.instance.transform.position = player_pos.position;
        wave_list = CombatManager.instance.wave_list;
        now_wave = 0;
        CombatManager.instance.enemy_num = 0;
    }
    
    void Update(){
        if(CombatManager.instance.enemy_num == 0){
            if(now_wave == wave_list.Count)CombatManager.instance.Combat_end();
            else StartCoroutine(Enemy_gen());
        }
    }
}