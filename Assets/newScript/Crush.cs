using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour
{
    public int crush_hurt = 10;
    public float crush_time_interval = 1f;
    float crush_time_count = 0f;

    bool init_finish = false;

    public void SetHurt(int new_hurt){
        crush_hurt = new_hurt;
    }

    public void SetTimeInterval(float new_time_interval){
        crush_time_interval = new_time_interval;
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
        if (!init_finish){
            crush_time_count += Time.deltaTime;
            if (crush_time_count > crush_time_interval){
                init_finish = true;
                crush_time_count = 0f;
            }
        }
    }


    void OnTriggerStay2D(Collider2D other) {
        Player a = other.gameObject.GetComponent<Player>();
        // Debug.Log(a);
        if (a != null){
            // if (crush_time_count <= crush_time_interval){
            //     crush_time_count += Time.deltaTime;
            // }
            // else{
                // Debug.Log("crush hurt " + crush_hurt.ToString());
            if (init_finish){
                a.ChangeHealth(-crush_hurt);
            }
            // }
        }
    }

    public void des(){
        Destroy(gameObject);
    }
}
