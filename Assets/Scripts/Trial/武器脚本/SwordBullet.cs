using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    public bool is_projectile;

    public float exist_time;
    float now_time;


    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
    }

    public void Launch(Vector2 direction, float speed)
    {
        rigidbody2d.velocity = direction * speed;
        transform.right = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //���ǻ������˵�����־���˽�ɵ��������Ķ���
        Enemy a = other.gameObject.GetComponent<Enemy>();
        if (a != null){
            a.Change_health(-1);
        }
        //Debug.Log("Projectile Collision with " + other.gameObject);
        if(is_projectile == true){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (now_time < 0)
        {
            Destroy(gameObject);
        }
        now_time -= Time.deltaTime;
    }
}
