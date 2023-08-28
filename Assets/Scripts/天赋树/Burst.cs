using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{

    public GameObject burst_effect;


    int Judge(BaseBoss a, float[] charge_now){
        int count1=0;   //已满的能量数
        int temp1=-1,temp2=-1;  //前两个已满的能量种类
        float sum=0;    //总能量值
        for(int i=0;i<5;i++){
            if(charge_now[i] >= 1){
                count1++;
                if(temp1 == -1){
                    temp1 = i;
                }
                else{
                    temp2 = i;
                }
            }
            sum+=charge_now[i];
        }
        if(count1 == 0)return -1;
        if(charge_now[4] == 1 && charge_now[4] >= sum-1){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*10*Player.instance.damage_boost),false);
        }
        else if(charge_now[4] == 1){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*5*Player.instance.damage_boost),false);
        }
        else if(count1 == 4){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*0.1f*Player.instance.damage_boost),false);
        }
        else if(count1 == 3){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*0.5f*Player.instance.damage_boost),false);
        }
        else if(count1 == 2 && 4*(sum-2)<=2){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*8*Player.instance.damage_boost),false);
        }
        else if(count1 == 2 && 2*(sum-2)<=2){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*4*Player.instance.damage_boost),false);
        }
        else if(count1 == 2){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*2*Player.instance.damage_boost),false);
        }
        else if(count1 == 1 && 2*(sum-1)<=1){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*6*Player.instance.damage_boost),false);
        }
        else if(count1 == 1 && sum<=2){
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*2*Player.instance.damage_boost),false);
        }
        else{
            a.ChangeHealth(-(int)(TalentManager.instance.burst_base*Player.instance.damage_boost),false);
        }
        a.ChargeClear();
        if(temp1 != -1 && temp2 == -1){
            CombatManager.instance.bursted[temp1] = true;
            return temp1;
        }
        return 5;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other){
        BaseBoss a = other.GetComponent<BaseBoss>();
        if(a!=null){
            int temp = Judge(a, a.charge_now);
            if(temp != -1){
                Vector2 dir = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y).normalized;
                a.ChangePos(0.5f * dir);
                GameObject temp11 = Instantiate(burst_effect, a.transform.position, Quaternion.identity);
                temp11.GetComponent<Burst_Effect>().init(temp);
            }
        }
    }

    void Destroy(){
        Destroy(gameObject);
    }
}
