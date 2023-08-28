using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuBullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    public float exist_time;
    float now_time;
    YuSword ys;

    float[] charge = new float[5];
    int damage;

    public float speed;

    public void init(int x, float[] y, Vector2 z, YuSword t){
        damage = x;
        for(int i = 0; i < 5; i ++){
            charge[i] = y[i];
        }
        dir = z;
        ys = t;
    }

    void Start(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = dir * speed;
        transform.right = dir.normalized;
        now_time = exist_time;
    }

    void OnCollisionEnter2D(Collision2D other){
        BaseBoss temp = other.gameObject.GetComponent<BaseBoss>();
        if(temp != null){
            temp.ChangeHealth(-damage,false);
            temp.ChargeEn(charge,false);
            temp.ChangePos(0.5f*dir);
            ys.damage_value.Enqueue(damage);
            ys.damage_time.Enqueue(CombatManager.instance.total_time);
            ys.total_damage += damage;
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update(){
        now_time = Mathf.Max(0, now_time - Time.deltaTime);
        if(now_time <= 0){
            Destroy(gameObject);
        }
    }
}
