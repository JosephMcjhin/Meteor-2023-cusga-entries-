using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject bullet;

    public Transform bullet_position;

    Vector2 mousePos;
    Vector2 lookDirection;
    Vector3 flip;
    Animator animator;

    public GameObject projectileObject;
    public Arrow projectile;

    float now_speed;
    float now_time;

    public float max_speed;
    public float max_time;

    Vector2 rotate(Vector2 now, float angle){
        Vector2 temp;
        angle = angle * Mathf.PI / 180;
        temp.x = now.x * Mathf.Cos(angle) + now.y * Mathf.Sin(angle);
        temp.y = -now.x * Mathf.Sin(angle) + now.y * Mathf.Cos(angle);
        return temp;
    }

    void Start(){
        animator = GetComponent<Animator>();
    }

    void Update(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        transform.right = lookDirection;
        if (Input.GetMouseButtonDown(0)){
            animator.SetTrigger("Start");
            now_speed = 0;
            now_time = 0;
            Launch();
            projectileObject.transform.position = transform.position;
        }
        else if (Input.GetMouseButton(0)){
            now_speed += Time.deltaTime/2f * max_speed;
            now_speed = Mathf.Min(now_speed, max_speed);
            now_time += Time.deltaTime/2f * max_time;
            now_time = Mathf.Min(now_time, max_time);
            projectile.Launch(lookDirection);
            projectileObject.transform.position = transform.position;
        }
        else if (Input.GetMouseButtonUp(0)){
            animator.SetTrigger("End");
            Launch1();
        }
        //animator.SetFloat("Look X", lookDirection.x);
        //Debug.Log(lookDirection.x);
    }

    void Launch(){
        projectileObject = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        projectile = projectileObject.GetComponent<Arrow>();
        //animator.SetTrigger("Launch");
    }
    void Launch1(){
        projectile.speed = now_speed;
        projectile.exist_time = now_time;
        projectile.now_time = now_time;
        projectile.start = true;
        float bias = Random.Range(-50*(max_time-now_time), 50*(max_time-now_time));
        projectile.Launch(Quaternion.AngleAxis(bias,Vector3.forward)*lookDirection);
    }
}
