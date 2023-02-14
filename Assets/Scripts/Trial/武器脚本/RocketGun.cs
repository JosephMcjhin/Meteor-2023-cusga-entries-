using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGun : MonoBehaviour
{
    public float bullet_time;
    float bullet_now;
    public GameObject bullet;

    public Transform bullet_position;

    Vector2 mousePos;
    Vector2 lookDirection;
    Vector3 flip;
    Animator animator;

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
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x < transform.position.x){
            transform.localScale = flip;
        }
        else{
            transform.localScale = new Vector3(-flip.x, flip.y, flip.z);
        }
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        if(lookDirection.x < 0){
            transform.right = new Vector2(-lookDirection.x,-lookDirection.y);
        }
        else{
            transform.right = lookDirection;
        }
        bullet_now -= Time.deltaTime;
        bullet_now = Mathf.Max(bullet_now,0);
        if (Input.GetMouseButtonDown(0)){
            if (bullet_now == 0){
                animator.SetTrigger("Shoot");
                bullet_now = bullet_time;
            }
        }
        //animator.SetFloat("Look X", lookDirection.x);
        //Debug.Log(lookDirection.x);
    }

    void Launch(float angle){
        GameObject projectileObject = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        Rocket projectile = projectileObject.GetComponent<Rocket>();
        projectile.Launch(Quaternion.AngleAxis(angle,Vector3.forward)*lookDirection);
        projectile.targetpos = mousePos;
        //animator.SetTrigger("Launch");
    }
    void Launch3(){
        Launch(40);
        Launch(20);
        Launch(0);
        Launch(-20);
        Launch(-40);
    }
}
