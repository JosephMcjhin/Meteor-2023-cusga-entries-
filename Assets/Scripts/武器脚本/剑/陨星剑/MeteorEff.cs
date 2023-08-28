using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEff : MonoBehaviour
{
    int damage;
    float[] charge = new float[5];
    SpriteRenderer co;
    public void init(int x, float[] y){
        damage = x;
        for(int i = 0; i < 5; i ++){
            charge[i] = y[i];
        }
    }
    
    IEnumerator Sign_dis(){
        for(float f = 1; f >= 0; f -= 0.05f){
            co.color = new Color(co.color.r,co.color.g,co.color.b,f);
            //print(12345);
            yield return new WaitForSeconds(.1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        BaseBoss temp = other.gameObject.GetComponent<BaseBoss>();
        if(temp != null){
            temp.ChangeHealth(-damage,false);
            temp.ChargeEn(charge,false);
        }
    }

    void Start(){
        co = GetComponent<SpriteRenderer>();
        StartCoroutine(Sign_dis());
    }
}
