using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    Animator animator;
    public float exist_time;
    float now_time;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
        dir = direction;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //我们还增加了调试日志来了解飞弹触碰到的对象
        Enemy a = other.gameObject.GetComponent<Enemy>();
        if (a != null)
        {
            a.Change_health(-1);
        }
        //Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (now_time < 0)
        {
            Destroy(gameObject);
        }
        animator.SetFloat("Look X", dir.x);
        animator.SetFloat("Look Y", dir.y);
        now_time -= Time.deltaTime;
    }
}
