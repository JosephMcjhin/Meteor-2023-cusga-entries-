using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuSword : MonoBehaviour
{
    public float combo_time;
    float combo_now;
    bool is_attack;
    int combo_flag;
    public GameObject damage_area;  //剑刃的范围
    public Transform wave_pos; //剑气
    public GameObject wave;
    public int base_damage;
    public float[] base_charge;

    public Transform bullet_position;   //剑波
    public GameObject bullet;

    //移动相关变量
    Vector2 lookDirection;
    Vector2 mousePos;

    IEnumerator Attack(){
        CombatManager.instance.switchable = false;

        is_attack = true;
        //生成攻击区域和剑气
        damage_area.SetActive(true);
        wave.SetActive(true);
        Vector3 temp11 = wave_pos.position;
        damage_area.GetComponent<Blade>().Init(TalentManager.instance.DamageFinal(base_damage, base_charge), TalentManager.instance.ChargeFinal(base_charge), lookDirection);

        //挥舞
        if(combo_flag == 1){
            for(float i = 120; i >= -120; i -= 24f){
                transform.right = Quaternion.AngleAxis(i, Vector3.forward) * lookDirection;
                wave.transform.right = lookDirection;
                wave.transform.position = temp11;
                if(i == 0){
                    Launch();
                }
                yield return new WaitForSeconds(0.02f);
            }
        }
        else{
            wave.transform.localScale = new Vector3(wave.transform.localScale.x, -wave.transform.localScale.y, wave.transform.localScale.z);
            for(float i = -120; i <= 120; i += 24f){
                transform.right = Quaternion.AngleAxis(i, Vector3.forward) * lookDirection;
                wave.transform.right = lookDirection;
                wave.transform.position = temp11;
                if(i == 0){
                    Launch();
                }
                yield return new WaitForSeconds(0.02f);
            }
            wave.transform.localScale = new Vector3(wave.transform.localScale.x, -wave.transform.localScale.y, wave.transform.localScale.z);
        }

        //后处理
        damage_area.SetActive(false);
        wave.SetActive(false);
        is_attack = false;
        combo_flag = -combo_flag;
        combo_now = combo_time;

        CombatManager.instance.switchable = true;
    }

    void Launch(){
        GameObject temp = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        float bias = Random.Range(-5f,5f);
        temp.GetComponent<YuBullet>().init(TalentManager.instance.DamageFinal(base_damage, base_charge), TalentManager.instance.ChargeFinal(base_charge), Quaternion.AngleAxis(bias,Vector3.forward)*lookDirection, GetComponent<YuSword>());
    }

    void Change_Dir(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.right = lookDirection;
    }

    //一段时间不受到伤害并且一定时间内造成大量伤害，获得云。
    bool is_fly;
    public Queue damage_value = new Queue();    //伤害计算
    public Queue damage_time = new Queue();
    public float total_damage;

    IEnumerator Fly(){
        CombatManager.instance.cloud.SetActive(true);
        is_fly = true;
        Player.instance.speed_boost += 1;
        yield return new WaitForSeconds(3f);
        CombatManager.instance.cloud.SetActive(false);
        is_fly = false;
        Player.instance.speed_boost -= 1;
    }

    void OnDisable(){
        StopCoroutine(Fly());
        CombatManager.instance.cloud.SetActive(false);
        if(is_fly)Player.instance.speed_boost -= 1;
        is_fly = false;
        damage_time.Clear();
        damage_value.Clear();
        total_damage = 0;
    }

    void Start(){
        combo_flag = 1;
        damage_area.SetActive(false);
        wave.SetActive(false);
    }

    void Change_queue(){
        if(damage_time.Count == 0)return;
        print(damage_time.Count == damage_value.Count);
        float temp = (float)damage_time.Peek();
        if(CombatManager.instance.total_time - temp >= 3f){
            int temp1 = (int)damage_value.Dequeue();
            total_damage -= temp1;
            damage_time.Dequeue();
        }
        if(total_damage >= 30f && CombatManager.instance.total_time - CombatManager.instance.last_hurt >= 5f && !is_fly){
            StartCoroutine(Fly());
        }
    }

    void Update(){
        if(is_attack)return;
        else{
            combo_now = Mathf.Max(0f, combo_now - Time.deltaTime);
            if(combo_now <= 0){
                combo_flag = 1;
            }
            Change_Dir();
            if(Input.GetMouseButtonDown(0)){
                StartCoroutine(Attack());
            }
        }
        Change_queue();
    }

}
