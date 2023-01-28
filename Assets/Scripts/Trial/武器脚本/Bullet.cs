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

    public float speed;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
    }

    public void Launch(Vector2 direction)
    {
        rigidbody2d.velocity = direction * speed;
        transform.right = direction.normalized;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //���ǻ������˵�����־���˽�ɵ��������Ķ���
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
