using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    int damage;
    float[] charge = new float[5];
    Vector3 target;
    Rigidbody2D rigidbody2d;

    public GameObject eff;
    public float speed;
    
    public void init(int x, float[] y, Vector3 z){
        damage = x;
        for(int i = 0; i < 5; i ++){
            charge[i] = y[i];
        }
        target = z;
    }

    void Start(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(0f, -1f) * speed;
    }

    void Update(){
        if(Vector3.Distance(transform.position,target)<=0.5f){
            GameObject temp = Instantiate(eff,target,Quaternion.identity);
            temp.GetComponent<MeteorEff>().init(damage,charge);
            Destroy(gameObject);
        }
    }
}
