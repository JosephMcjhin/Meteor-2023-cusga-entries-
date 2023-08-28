using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightSword : MonoBehaviour
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
                yield return new WaitForSeconds(0.02f);
            }
        }
        else{
            wave.transform.localScale = new Vector3(wave.transform.localScale.x, -wave.transform.localScale.y, wave.transform.localScale.z);
            for(float i = -120; i <= 120; i += 24f){
                transform.right = Quaternion.AngleAxis(i, Vector3.forward) * lookDirection;
                wave.transform.right = lookDirection;
                wave.transform.position = temp11;
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

    IEnumerator Bash(){
        CombatManager.instance.switchable = false;

        is_attack = true;
        CombatManager.instance.bursted[2] = false;

        //改变大小颜色
        Vector3 scale = transform.localScale;
        transform.localScale = 2 * scale;
        SpriteRenderer temp = GetComponent<SpriteRenderer>();
        Color co = temp.color;
        temp.color = new Color(co.r, 0, 0, co.a);

        //生成攻击区域和剑气
        damage_area.SetActive(true);
        wave.SetActive(true);
        Vector3 temp11 = wave_pos.position;
        damage_area.GetComponent<Blade>().Init(TalentManager.instance.DamageFinal(base_damage, base_charge) * 5, TalentManager.instance.ChargeFinal(base_charge), lookDirection);
        for(float i = 120; i >= -120; i -= 24f){
            transform.right = Quaternion.AngleAxis(i, Vector3.forward) * lookDirection;
            wave.transform.right = lookDirection;
            wave.transform.position = temp11;
            yield return new WaitForSeconds(0.02f);
        }

        //后处理
        damage_area.SetActive(false);
        wave.SetActive(false);
        temp.color = co;
        is_attack = false;
        transform.localScale = scale;

        CombatManager.instance.switchable = true;
    }

    void Change_Dir(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.right = lookDirection;
    }

    void Start(){
        combo_flag = 1;
        damage_area.SetActive(false);
        wave.SetActive(false);
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
                return;
            }
            if(CombatManager.instance.bursted[2] && Input.GetMouseButtonDown(1)){
                StartCoroutine(Bash());
            }
        }
    }

}
