using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSword : MonoBehaviour
{
    public float fire_time;
    float fire_now;
    public GameObject fire_eff;
    public Transform eff_position;
    bool is_attack;
    public int base_damage;
    public float[] base_charge;

    public GameObject meteor;

    //移动相关变量
    Vector2 lookDirection;
    Vector2 mousePos;

    IEnumerator Attack(){
        CombatManager.instance.switchable = false;
        is_attack = true;

        transform.right = new Vector3(0f, 1f, transform.right.z);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        GameObject temp = Instantiate(fire_eff, eff_position.position, Quaternion.identity);
        for(float i = 0; i < 5; i ++){
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.02f, transform.position.z);
            yield return new WaitForSeconds(0.04f);
        }
        for(float i = 0; i < 5; i ++){
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
            yield return new WaitForSeconds(0.04f);
        }
        Launch();
        while(Input.GetMouseButton(0)){
            for(float i = 0; i < 5; i ++){
                //transform.position = new Vector3(transform.position.x, transform.position.y - 0.02f, transform.position.z);
                yield return new WaitForSeconds(0.04f);
            }
            for(float i = 0; i < 5; i ++){
                //transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
                yield return new WaitForSeconds(0.04f);
            }
            Launch();
        }
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        Destroy(temp);
        is_attack = false;
        fire_now = fire_time;

        CombatManager.instance.switchable = true;
    }

    Collider2D[] enemy_in = new Collider2D[10];

    void Launch(){

        int temp = Physics2D.OverlapCircleNonAlloc(new Vector2(Player.instance.transform.position.x, Player.instance.transform.position.y), 10f, enemy_in, 1<<3, -100f, 100f);
        Vector3 temp1;
        if(temp == 0){
            temp1 = new Vector3(Random.Range(-10f,10f) + Player.instance.transform.position.x,Random.Range(-10f,10f) + Player.instance.transform.position.y,0f + Player.instance.transform.position.z);
        }
        else{
            int temp11 = Random.Range(0,temp);
            temp1 = enemy_in[temp11].gameObject.transform.position;
        }
        GameObject temp2 = Instantiate(meteor, new Vector3(temp1.x,temp1.y + 15f,temp1.z), Quaternion.identity);
        temp2.GetComponent<Meteor>().init(TalentManager.instance.DamageFinal(base_damage, base_charge), TalentManager.instance.ChargeFinal(base_charge), temp1);
    }

    void Change_Dir(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.right = lookDirection;
    }

    

    void Start(){
    }


    void Update(){
        if(is_attack)return;
        else{
            fire_now = Mathf.Max(0, fire_now - Time.deltaTime);
            Change_Dir();
            if(Input.GetMouseButtonDown(0) && fire_now <= 0){
                StartCoroutine(Attack());
            }
        }
    }

}
