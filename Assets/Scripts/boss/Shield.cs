using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update

    public float max_health;
    public float now_health;
    Animator animator;

    void OnEnable(){
        now_health = max_health;
        animator = GetComponent<Animator>();
    }

    public void Change_health(float x){
        now_health += x;
        now_health = Mathf.Max(0,now_health);
    }

}
