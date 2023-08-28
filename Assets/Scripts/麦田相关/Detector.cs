using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public GameObject detector_sign;
    public Vector3 treasure_pos;
    Vector2 mousePos;
    Vector2 lookDirection;
    Vector3 flip;

    float raw_dis;
    float now_dis;
    bool in_find;

    public bool is_found;
    

    SpriteRenderer co;

    IEnumerator Sign_dis(){
        //print(1234);
        while(true){
            float temp1 = raw_dis/now_dis;
            float temp2 = now_dis/raw_dis;
            for(float f = 0; f <= 1f; f += 0.05f*Mathf.Min(10f,temp1)){
                co.color = new Color(co.color.r,co.color.g,co.color.b,f);
                //print(12345);
                yield return new WaitForSeconds(.05f);
            }
            for(float f = 1f; f >= 0; f -= 0.05f*Mathf.Min(10f,temp1)){
                co.color = new Color(co.color.r,co.color.g,co.color.b,f);
                yield return new WaitForSeconds(.05f);
            }
            yield return new WaitForSeconds(2f*Mathf.Max(.05f,temp2));
        }
    }

    void Start(){
        co = detector_sign.GetComponent<SpriteRenderer>();
        detector_sign.SetActive(false);
        flip = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }

    void Dir_change(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x < transform.position.x){
            transform.localScale = flip;
        }
        else{
            transform.localScale = new Vector3(-flip.x, flip.y, flip.z);
        }
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        if(lookDirection.x < 0){
            transform.right = new Vector2(-lookDirection.x,-lookDirection.y);
        }
        else{
            transform.right = lookDirection;
        }
    }

    public void Start_find(Vector3 pos){
        in_find = true;
        treasure_pos = pos;
        raw_dis = (Player.instance.transform.position - treasure_pos).magnitude;
        raw_dis = Mathf.Max(2f,raw_dis);
        detector_sign.SetActive(true);
        StartCoroutine("Sign_dis");
    }

    public void Leave(){
        in_find = false;
        detector_sign.SetActive(false);
        StopCoroutine("Sign_dis");
    }

    void Update(){
        Dir_change();
        if(!in_find)return;
        now_dis = (Player.instance.transform.position - treasure_pos).magnitude;
        if(now_dis <= 5f){
            now_dis = 5f;
            if(Input.GetKeyDown("e")){
                Debug.Log("treasure found!");
                Player.instance.treasure_found ++;
                Debug.Log(Player.instance.treasure_found);
                detector_sign.SetActive(false);
                StopCoroutine("Sign_dis");
                is_found = true;
            }
        }
        //animator.SetFloat("Look X", lookDirection.x);
        //Debug.Log(lookDirection.x);
    }
}
