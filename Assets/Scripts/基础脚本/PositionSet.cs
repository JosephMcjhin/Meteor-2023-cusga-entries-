using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSet : MonoBehaviour
{
    public string now_sign;
    void Start(){
        if(Player.instance.main_sign == now_sign){
            Player.instance.transform.position = transform.position;
        }
    }
}
