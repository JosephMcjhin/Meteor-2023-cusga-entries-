using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;

    GameObject player;
    Rigidbody2D rigidbody2d;

    int flag = 0;

    Animator animator;
    Vector2 dir;

    public int max_health;
    int now_health;

    void Awake()
    {
        //player = player.transform.Find("Player").gameObject;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        now_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Move X", dir.x);
        animator.SetFloat("Move Y", dir.y);
    }

    void FixedUpdate()
    {
        if(flag == 0)player = GameObject.Find("Player");
        if(player == null)
        {
            return;
        }
        flag = 1;
        Vector2 position = rigidbody2d.position;
        dir.x = -position.x + player.transform.position.x;
        dir.y = -position.y + player.transform.position.y;
        dir = dir.normalized;
        position.x = position.x + speed * dir.x * Time.deltaTime;
        position.y = position.y + speed * dir.y * Time.deltaTime;
        //Debug.Log(dir);
        rigidbody2d.MovePosition(position);
    }

    public void Change_health(int x)
    {
        now_health += x;
        if (now_health <= 0)
        {
            Destroy(gameObject);
        }
        //Debug.Log(now_health);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        //我们还增加了调试日志来了解飞弹触碰到的对象
        Tank a = other.gameObject.GetComponent<Tank>();
        if (a != null)
        {
            a.Change_health(-1);
        }
        //Debug.Log("Projectile Collision with " + other.gameObject);
        //Destroy(gameObject);
    }
}
