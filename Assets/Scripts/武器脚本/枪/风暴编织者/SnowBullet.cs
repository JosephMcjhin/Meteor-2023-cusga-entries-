using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    public float exist_time;
    float now_time;

    float[] charge = new float[5];
    int damage;

    public float speed;

    public void init(int x, float[] y, Vector2 z){
        damage = x;
        for(int i = 0; i < 5; i ++){
            charge[i] = y[i];
        }
        dir = z;
    }

    void Start(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = dir * speed;
        transform.right = dir.normalized;
        now_time = exist_time;
    }

    void OnCollisionEnter2D(Collision2D other){
        BaseBoss a = other.gameObject.GetComponent<BaseBoss>();
        if(a != null){
            a.ChangeHealth(-damage,false);
            a.ChargeEn(charge,false);
            a.ChangePos(0.5f*dir);
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
