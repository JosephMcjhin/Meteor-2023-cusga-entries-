using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower_Trigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        Player a = other.gameObject.GetComponent<Player>();
        if(a != null){
            Flower_Manager.instance.Scene_Start();
        }
    }

    void OnTriggerExit2D(Collider2D other){
        Player a = other.gameObject.GetComponent<Player>();
        if(a != null){
            Flower_Manager.instance.Scene_End();
        }
    }
}
