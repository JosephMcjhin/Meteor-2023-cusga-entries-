using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Flower_Manager : MonoBehaviour
{
    public static Flower_Manager instance;
    void Awake(){
        if(instance!=null){
            Destroy(this);
        }
        instance = this;
    }

    [TextArea]
    public string[] text;
    public Passive_Spawn[] spawn;
    public Sprite circle;
    public GameObject circle_ren;

    int state;

    bool wait_next;

    public GameObject bk;
    public TextMeshProUGUI text_now;

    public GameObject stone;

    IEnumerator Wait_Start(){
        yield return new WaitForSeconds(3f);
        circle_ren.GetComponent<SpriteRenderer>().sprite = circle;
        state = 1;
    }

    IEnumerator Display(int text_num, int next_state, float wait_time){
        bk.SetActive(true);
        text_now.text = "";
        for(int j = 0; j < text[text_num].Length; j ++){
            text_now.text += text[text_num][j];
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(2f);
        text_now.text = "";
        bk.SetActive(false);
        yield return new WaitForSeconds(wait_time);
        state = next_state;
        wait_next = false;
        //gameObject.SetActive(false);
    }

    public void Scene_Start(){
        StartCoroutine("Wait_Start");
    }

    public void Scene_End(){
        StopAllCoroutines();
        circle_ren.GetComponent<SpriteRenderer>().sprite = null;
        if(state > 0)StartCoroutine(Display(5,0,0f));
    }


    void Update(){
        if(state == 1 && !wait_next){
            wait_next = true;
            StartCoroutine(Display(0,2,2f));
        }
        else if(state == 2 && !wait_next){
            wait_next = true;
            StartCoroutine(Display(1,3,1f));
            for(int i = 0; i < spawn.Length; i++){
                spawn[i].Enemy_spawn();
            }
        }
        else if(state == 3 && !wait_next){
            wait_next = true;
            StartCoroutine(Display(2,4,1f));
            for(int i = 0; i < spawn.Length; i++){
                spawn[i].Enemy_spawn();
            }
        }
        else if(state == 4 && !wait_next){
            wait_next = true;
            StartCoroutine(Display(3,5,1f));
            for(int i = 0; i < spawn.Length; i++){
                spawn[i].Enemy_spawn();
            }
        }
        else if(state == 5 && !wait_next){
            wait_next = true;
            Destroy(stone);
            DestroyImmediate(circle_ren);
            StartCoroutine(Display(4,0,0f));
        }
    }
}
