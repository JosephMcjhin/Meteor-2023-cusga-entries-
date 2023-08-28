using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGun : MonoBehaviour
{
    public float fire_time;
    float fire_now;
    float cu_fire_time;
    public GameObject bullet;
    public GameObject fire_eff;
    public Transform bullet_position;
    bool is_attack;

    public int damage;
    public float[] charge;

    //移动相关变量
    Vector2 lookDirection;
    Vector2 mousePos;
    Vector3 flip;

    IEnumerator Attack(){
        CombatManager.instance.switchable = false;

        is_attack = true;
        for(int i = 0; i < 5; i ++){
            transform.position = new Vector3(transform.position.x - 0.02f * lookDirection.x, transform.position.y - 0.02f * lookDirection.y, transform.position.z);
            yield return new WaitForSeconds(0.04f * cu_fire_time / fire_time);
        }
        fire_eff.SetActive(true);
        Launch();
        for(int i = 0; i < 5; i ++){
            transform.position = new Vector3(transform.position.x + 0.02f * lookDirection.x, transform.position.y + 0.02f * lookDirection.y, transform.position.z);
            yield return new WaitForSeconds(0.04f * cu_fire_time / fire_time);
        }
        fire_eff.SetActive(false);
        is_attack = false;
        fire_now = cu_fire_time;

        CombatManager.instance.switchable = true;
    }
 
    void Change_Dir(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        if(lookDirection.x < 0) transform.right = new Vector2(-lookDirection.x,-lookDirection.y);
        else transform.right = lookDirection;
        if(mousePos.x < Player.instance.transform.position.x)transform.localScale = new Vector3(-flip.x, flip.y, flip.z);
        else transform.localScale = new Vector3(flip.x, flip.y, flip.z);
    }

    void OnEnable(){
        cu_fire_time = fire_time;
    }

    void Start(){
        fire_eff.SetActive(false);
        cu_fire_time = fire_time;
        flip = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }

    void Update(){
        if(is_attack){
            cu_fire_time = Mathf.Max(0.1f, cu_fire_time - Time.deltaTime/2f);
        }
        else{
            fire_now = Mathf.Max(0, fire_now - Time.deltaTime);
            Change_Dir();
            if(Input.GetMouseButton(0) && fire_now <= 0){
                StartCoroutine(Attack());
            }
            else if(!Input.GetMouseButton(0)){
                cu_fire_time = fire_time;
            }
        }
    }

    void Launch(){
        GameObject temp = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        float bias = Random.Range(-20f,20f);
        temp.GetComponent<SnowBullet>().init(TalentManager.instance.DamageFinal(damage, charge), TalentManager.instance.ChargeFinal(charge), Quaternion.AngleAxis(bias,Vector3.forward)*lookDirection);
    }
    /*
    void Launch(float angle){
        GameObject projectileObject = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        float bias = Random.Range(-5f,5f);
        Bullet projectile = projectileObject.GetComponent<Bullet>();
        projectile.Launch(Quaternion.AngleAxis(bias+angle,Vector3.forward)*lookDirection);
        //animator.SetTrigger("Launch");
    }
    void Launch3(){
        Launch(0);
        Launch(-35);
        Launch(35);
    }
    */
}
