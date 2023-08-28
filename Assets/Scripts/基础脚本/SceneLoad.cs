using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public int[] time_spot;
    public GameObject[] enemy_list;
    public GameObject[] object_list;

    void Start(){
        for(int i=0;i<time_spot.Length;i++){
            if(Player.instance.now_timeline<=time_spot[i]){
                enemy_list[i].SetActive(true);
                object_list[i].SetActive(true);
                for(int j=0;j<enemy_list[i].transform.childCount;j++){
                    if(CombatManager.instance.enemy_dead[j])enemy_list[i].transform.GetChild(j).gameObject.SetActive(false);
                }
                break;
            }
        }
    }
}
