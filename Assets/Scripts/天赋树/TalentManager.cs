using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalentManager : MonoBehaviour
{
    public static TalentManager instance;
    void Awake(){
        if(instance!=null){
            Destroy(this);
        }
        instance = this;
    }

    public TextMeshProUGUI desc;
    public GameObject button;
    public Talent talent;
    public GameObject[] locked;
    int now_dis;
    public void Refresh(){
        desc.text = "";
        button.SetActive(false);
        for(int i=0;i<5;i++){
            locked[i].SetActive(true);
        }
        if(talent.talent_level[1] > 0){
            locked[0].SetActive(false);
        }
        if(talent.talent_level[2] > 0){
            locked[1].SetActive(false);
            locked[3].SetActive(false);
        }
        locked[2].SetActive(false);
        if(talent.talent_level[3] > 0){
            locked[4].SetActive(false);
        }
        Player.instance.damage_boost = 0.2f * talent.talent_level[2];
    }

    public void Display(int x){
        now_dis = x;
        desc.text = talent.talent_info[x] + "\n当前等级:" + talent.talent_level[x].ToString();
        button.SetActive(true);
    }

    public void Study(){
        if(talent.talent_point == 0 || talent.talent_level[now_dis] == talent.max_level[now_dis])return;
        talent.talent_level[now_dis] ++;
        talent.talent_point --;
        Refresh();
    }

    public GameObject burst_prefab;    //元素引爆
    public int burst_base;
    public float burst_time;    
    float burst_now;
    public int recover_base;
    public float recover_time;
    float recover_now;
    public int[] qinhe = new int[5];

    public void QinheClear(){
        qinhe = new int[5];
    }

    IEnumerator Buff1(){
        Player.instance.speed_boost += 0.5f;
        yield return new WaitForSeconds(1.5f);
        Player.instance.speed_boost -= 0.5f;
    }

    IEnumerator Buff2(){
        Player.instance.recover += 10;
        yield return new WaitForSeconds(1.5f);
        Player.instance.recover -= 10;
    }

    IEnumerator Buff3(){
        Player.instance.damage_boost += 0.5f;
        yield return new WaitForSeconds(1.5f);
        Player.instance.damage_boost -= 0.5f;
    }

    IEnumerator Buff4(){
        Player.instance.attack_speed += 0.5f;
        yield return new WaitForSeconds(1.5f);
        Player.instance.attack_speed -= 0.5f;
    }

    IEnumerator Buff5(){
        Player.instance.attack_boost += 0.5f;
        yield return new WaitForSeconds(1.5f);
        Player.instance.attack_boost -= 0.5f;
    }

    IEnumerator Buff6(int temp){
        Player.instance.now_shield += 100*(temp+1);
        Player.instance.shield_sign.SetActive(true);
        Player.instance.shield_sign.GetComponent<ShieldEff>().init(temp);
        yield return new WaitForSeconds(1.5f);
        Player.instance.now_shield = 0;
    }

    void Qinhe(int now){
        if(qinhe[now] >= 1){
            if(now == 0){
                StartCoroutine(Buff1());
            }
            else if(now == 1){
                StartCoroutine(Buff2());
            }
            else if(now == 2){
                StartCoroutine(Buff3());
            }
            else if(now == 3){
                StartCoroutine(Buff4());
            }
            else{
                StartCoroutine(Buff5());
            }
        }
        if(qinhe[now] >= 2){
            StartCoroutine(Buff6(now));
        }
    }

    void ChangeTime(){
        burst_now = Mathf.Max(burst_now - Time.deltaTime, 0);
        recover_now = Mathf.Max(recover_now - Time.deltaTime, 0);
    }

    public void Recover(float[] charge_now){
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
        if(count1 == 0)return;
        Player.instance.recover_sign.SetActive(true);
        if(count1 >= 2){
            Player.instance.recover_sign.GetComponent<ReCover>().init(5);
            Player.instance.ChangeHealth(recover_base * (count1-1));
        }
        else{
            Player.instance.recover_sign.GetComponent<ReCover>().init(temp1);
            Player.instance.ChangeHealth(recover_base * (temp1+1));
            if(talent.talent_level[0] > 0){
                Qinhe(temp1);
            }
        }
        Player.instance.ChargeClear();
    }

    public float[] ChargeFinal(float[] charge){
        float[] result = new float[5];
        if(talent.talent_level[4] > 0){
            return result;
        }
        for(int i=0;i<5;i++){
            if(talent.talent_level[2]-1<i){
                result[i] = 0;
            }
            else{
                result[i] = charge[i];
            }
        }
        return result;
    }

    public int DamageFinal(int damage, float[] charge){
        int result = damage;
        result = (int)(result*Player.instance.attack_boost);
        if(talent.talent_level[4] == 0){
            return result;
        }
        for(int i=0;i<5;i++){
            result += (int)(charge[i] * 20 * (i+1) * Player.instance.damage_boost);
        }
        return result;
    }

    void Button(){
        if(Input.GetKeyDown("b")){
            if(burst_now == 0 && talent.talent_level[3] > 0){
                Instantiate(burst_prefab,Player.instance.mousePos,Quaternion.identity);
                burst_now = burst_time;
            }
        }
        if(Input.GetKeyDown("q")){
            if(recover_now == 0 && talent.talent_level[1] > 0){
                Recover(Player.instance.charge_now);
                recover_now = recover_time;
            }
        }
    }

    void Update(){
        ChangeTime();
        Button();
    }

}
