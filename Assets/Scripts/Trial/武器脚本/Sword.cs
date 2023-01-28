using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float bullet_time;
    float bullet_now;
    public GameObject bullet;

    public Transform bullet_position;

    Vector2 mousePos;
    Vector2 lookDirection;
    Vector3 flip;
    Animator animator;
    GameObject projectileObject=null;

    Vector2 rotate(Vector2 now, float angle){
        Vector2 temp;
        angle = angle * Mathf.PI / 180;
        temp.x = now.x * Mathf.Cos(angle) + now.y * Mathf.Sin(angle);
        temp.y = -now.x * Mathf.Sin(angle) + now.y * Mathf.Cos(angle);
        return temp;
    }

    void Start(){
        animator = GetComponent<Animator>();
        flip = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }

    void Update(){
        //Debug.Log(123);
        AnimatorStateInfo animatorInfo;
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        bullet_now -= Time.deltaTime;
        bullet_now = Mathf.Max(bullet_now,0);
        if(animatorInfo.IsName("攻击")){
            lookDirection = Quaternion.AngleAxis(-720*Time.deltaTime,Vector3.forward)*lookDirection;
            transform.right = lookDirection;
            //Debug.Log(123);
            return;
        }
        if(projectileObject!=null){
            Destroy(projectileObject);
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.right = lookDirection;
        if (Input.GetMouseButtonDown(0)){
            if (bullet_now == 0){
                animator.SetTrigger("Shoot");
                Launch(0f);
                bullet_now = bullet_time;
            }
        }
        if (Input.GetMouseButtonDown(1)){
            if (bullet_now == 0){
                animator.SetTrigger("Shoot");
                Launch(10f);
                bullet_now = bullet_time;
            }
        }
        //animator.SetFloat("Look X", lookDirection.x);
        //Debug.Log(lookDirection.x);
    }

    void Launch(float speed){
        projectileObject = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        SwordBullet projectile = projectileObject.GetComponent<SwordBullet>();
        if(speed != 0){
            projectile.is_projectile = true;
        }
        projectile.Launch(Quaternion.AngleAxis(0,Vector3.forward)*lookDirection, speed);
        /*
        AnimatorStateInfo animatorInfo;
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if((animatorInfo.normalizedTime >= 1.0f) && projectileObject != null){
            Destroy(projectileObject);
        }
        */
        //animator.SetTrigger("Launch");
    }
}
