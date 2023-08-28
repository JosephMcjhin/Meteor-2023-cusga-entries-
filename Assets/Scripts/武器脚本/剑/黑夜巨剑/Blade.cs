using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    int damage;
    float[] charge = new float[5];
    Vector2 dir;

    public void Init(int x, float[] y, Vector2 z){
        damage = x;
        for(int i = 0; i < 5; i ++){
            charge[i] = y[i];
        }
        dir = z;
    }

    void OnTriggerEnter2D(Collider2D other){
        BaseBoss a = other.gameObject.GetComponent<BaseBoss>();
        if(a != null){
            a.ChangeHealth(-damage,false);
            a.ChargeEn(charge,false);
            a.ChangePos(0.5f*dir);
        }
    }
}
