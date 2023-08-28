using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{

    public float life_time;
    float life_time_count = 0;

   void Awake()
    {
        life_time_count = life_time;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
        if (life_time_count < 0)
        {
            Destroy(gameObject);    
        }
        else
        {
            life_time_count -= Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D other){
        Player a= other.GetComponent<Player>();
        if(a!=null){
            a.ChangeHealth(-1);
        }
    }
}
