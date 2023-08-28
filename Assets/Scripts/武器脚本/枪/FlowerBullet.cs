using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    public float exist_time;
    float now_time;

    float[] charge = new float[5];
    int damage;

    public float speed;

    public GameObject FlowerDamage;

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
        GameObject temp = Instantiate(FlowerDamage, transform.position, Quaternion.identity);
        temp.GetComponent<FlowerDamage>().init(damage, charge);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update(){
        now_time = Mathf.Max(0, now_time - Time.deltaTime);
        if(now_time <= 0){
            GameObject temp = Instantiate(FlowerDamage, transform.position, Quaternion.identity);
            temp.GetComponent<FlowerDamage>().init(damage, charge);
            Destroy(gameObject);
        }
    }
}
