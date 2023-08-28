using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst_Effect : MonoBehaviour
{
    Animator aa;
    int state;
    public void init(int x){
        state = x;
    } 
    void Start(){
        aa = GetComponent<Animator>();
        aa.SetInteger("State", state);
    }

    void Destroy(){
        Destroy(gameObject);
    }
}
