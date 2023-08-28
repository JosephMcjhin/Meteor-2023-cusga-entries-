using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSign : MonoBehaviour
{
    SpriteRenderer co;
    bool is_locked;
    IEnumerator Sign_dis(){
        //print(1234);
        is_locked = true;
        while(CombatManager.instance.bursted[2]){
            for(float f = 1; f >= 0; f -= 0.05f){
                co.color = new Color(co.color.r,co.color.g,co.color.b,f);
                //print(12345);
                yield return new WaitForSeconds(.05f);
            }
            for(float f = 0; f <= 1; f += 0.05f){
                co.color = new Color(co.color.r,co.color.g,co.color.b,f);
                yield return new WaitForSeconds(.05f);
            }
        }
        is_locked = false;
    }

    void Start(){
        co = GetComponent<SpriteRenderer>();
        co.color = new Color(co.color.r,co.color.g,co.color.b,0);
        is_locked = false;
    }

    void Update(){
        if(CombatManager.instance.bursted[2] && !is_locked){
            StartCoroutine("Sign_dis");
        }
        else if(!CombatManager.instance.bursted[2]){
            StopCoroutine("Sign_dis");
            is_locked = false;
            co.color = new Color(co.color.r,co.color.g,co.color.b,0);
        }
    }
}
