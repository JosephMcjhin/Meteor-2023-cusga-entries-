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

    int bullet_type = 0;

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

    public void SetType(int type){
        bullet_type = type;
    }
    public int GetBulletType(){
        return bullet_type;
    }

    public void SetExistTime(float new_time)
    {
        exist_time = new_time;
        now_time = exist_time;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // player射向敌人的子弹

        // 敌人射向player的子弹
        if (bullet_type == 1){
            Player a = other.gameObject.GetComponent<Player>();
            if (a != null){
                a.ChangeHealth(-1);
                Destroy(gameObject);
            }

        }
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
