using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QinheTrigger : MonoBehaviour
{
    public float recover_time;
    float recover_now;
    public float[] charge;
    public int[] qinhe_value;

    bool inplace;

    private void OnTriggerEnter2D(Collider2D other){
        print(333444);
        if(other.gameObject.CompareTag("Player")){
            inplace = !inplace;
            if(inplace){
                for(int i=0;i<5;i++){
                    TalentManager.instance.qinhe[i] += qinhe_value[i];
                }
            }
            else{
                for(int i=0;i<5;i++){
                    TalentManager.instance.qinhe[i] -= qinhe_value[i];
                }
            }
        }
    }

    void OnDisable(){
        TalentManager.instance.QinheClear();
    }

    void Update(){
        recover_now -= Time.deltaTime;
        recover_now = Mathf.Max(0f, recover_now);
        if(inplace && recover_now <= 0){
            Player.instance.ChargeEn(charge);
            recover_now = recover_time;
        }
    }
}
