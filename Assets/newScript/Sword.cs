using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Shell
{
    // Start is called before the first frame update
    // Rigidbody2D rigidbody2d;
    // Vector2 dir;
    // Vector2 current_position;
    // public float speed = 1f;

    // // Animator animator;
    // public float exist_time;
    // float now_time;

    // int bullet_type = 0;

    void Awake()
    {
        // animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
    }

    // public void Launch(Vector2 direction, float force)
    // {
    //     rigidbody2d.AddForce(direction * force);
    //     dir = direction;
    // }


    // public void SetExistTime(float new_time)
    // {
    //     exist_time = new_time;
    //     now_time = exist_time;
    // }

    // public void SetSpeed(float new_speed)
    // {
    //     speed = new_speed;
    // }

    void OnCollisionEnter2D(Collision2D other)
    {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null){
            a.ChangeHealth(-damage);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (now_time < 0)
        {
            Destroy(gameObject);
        }
        // animator.SetFloat("Look X", dir.x);
        // animator.SetFloat("Look Y", dir.y);
        dir = dir.normalized;
        current_position = rigidbody2d.position;
        current_position.x = current_position.x + speed * dir.x * Time.deltaTime;
        current_position.y = current_position.y + speed * dir.y * Time.deltaTime;
        rigidbody2d.MovePosition(current_position);
        now_time -= Time.deltaTime;
    }
}
