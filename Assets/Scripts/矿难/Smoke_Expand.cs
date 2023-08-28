using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke_Expand : MonoBehaviour
{
    public float expand_speed;

    void OnTriggerStay2D(Collider2D other){
        Player a = other.gameObject.GetComponent<Player>();
        if(a != null){
            a.ChangeHealth(-1);
        }
    }

    void FixedUpdate(){
        transform.localScale = new Vector3(transform.localScale.x + expand_speed, transform.localScale.y + expand_speed, transform.localScale.z);
    }
}
