using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
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

    Vector3 temp;

    void Update(){
        //Debug.Log(123);
        AnimatorStateInfo animatorInfo;
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        bullet_now -= Time.deltaTime;
        bullet_now = Mathf.Max(bullet_now,0);
        if(animatorInfo.IsName("出击")){
            temp = temp + new Vector3(lookDirection.x * (Time.deltaTime/0.25f),lookDirection.y * (Time.deltaTime/0.25f),0);
            transform.position = transform.parent.position + temp;
            projectileObject.transform.position = bullet_position.position;
            //Debug.Log(123);
            return;
        }
        if(projectileObject!=null){
            Destroy(projectileObject);
        }
        transform.position = transform.parent.position;
        temp = new Vector3(0,0,0);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.right = lookDirection;
        if (Input.GetMouseButtonDown(0)){
            if (bullet_now == 0){
                animator.SetTrigger("Shoot");
                Launch();
                bullet_now = bullet_time;
            }
        }
        //animator.SetFloat("Look X", lookDirection.x);
        //Debug.Log(lookDirection.x);
    }

    void Launch(){
        projectileObject = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        SpearBullet projectile = projectileObject.GetComponent<SpearBullet>();
        projectile.Launch(Quaternion.AngleAxis(0,Vector3.forward)*lookDirection);
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
