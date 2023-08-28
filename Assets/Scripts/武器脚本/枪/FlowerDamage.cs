using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerDamage : MonoBehaviour
{
    int damage;
    float[] charge = new float[5];

    public float exist_time;
    float now_time;

    public void init(int x, float[] y){
        damage = x;
        for(int i = 0; i < 5; i ++){
            charge[i] = y[i];
        }
    }

    void Start(){
        now_time = exist_time;
    }

    void Update(){
        now_time = Mathf.Max(0, now_time - Time.deltaTime);
        if(now_time <= 0){
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        BaseBoss a = other.gameObject.GetComponent<BaseBoss>();
        if(a != null){
            a.ChangeHealth(-damage,true);
            a.ChargeEn(charge,true);
        }
    }
}
