using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    float x1,x2,y1,y2;
    BoxCollider2D temp;
    bool in_place;
    Detector a;
    void Start(){
        temp = GetComponent<BoxCollider2D>();
        x1 = transform.position.x - temp.size.x/2f;
        x2 = transform.position.x + temp.size.x/2f;
        y1 = transform.position.y - temp.size.y/2f;
        y2 = transform.position.y + temp.size.y/2f;
    }

    void OnTriggerEnter2D(Collider2D other){
        Detector aa = other.gameObject.GetComponent<Detector>();
        //print(a);
        if(aa != null){
            a = aa;
            in_place = true;
            float temp1 = Random.Range(x1,x2);
            float temp2 = Random.Range(y1,y2);
            a.Start_find(new Vector3(temp1,temp2,0));
        }
    }

    void OnTriggerExit2D(Collider2D other){
        Detector aa = other.gameObject.GetComponent<Detector>();
        if(aa != null){
            a = aa;
            in_place = false;
            a.is_found = false;
            a.Leave();
        }
    }

    void Update(){
        //print(a);
        if(a != null && in_place){
            //print(a.is_found);
            if(a.is_found == true){
                //print(12345);
                float temp1 = Random.Range(x1,x2);
                float temp2 = Random.Range(y1,y2);
                a.Start_find(new Vector3(temp1,temp2,0));
                a.is_found = false;
            }
        }
        else if(a == null){
            in_place = false;
        }
    }
}
