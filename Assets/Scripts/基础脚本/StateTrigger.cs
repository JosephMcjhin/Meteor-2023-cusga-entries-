using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTrigger : MonoBehaviour
{
    public int state_id;
    public int time_line;
    public GameObject trigger_object;
    void Start(){
        if(Player.instance.player_state.state_list[state_id] && Player.instance.now_timeline <= time_line){
            trigger_object.SetActive(true);
        }
        else{
            trigger_object.SetActive(false);
        }
    }
}
