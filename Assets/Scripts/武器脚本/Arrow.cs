using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    public float exist_time;
    public float now_time;

    public float speed;

    public bool start;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
    }

    public void Launch(Vector2 direction)
    {
        //Debug.Log(speed);
        rigidbody2d.velocity = direction * speed;
        transform.right = direction.normalized;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(999);
        if(!start){
            return;
        }
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
        if(!start){
            return;
        }
        if (now_time < 0)
        {
            Destroy(gameObject);
        }
        now_time -= Time.deltaTime;
    }
}
