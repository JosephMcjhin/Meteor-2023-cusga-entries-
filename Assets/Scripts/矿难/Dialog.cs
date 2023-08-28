using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public GameObject bk;
    public TextMeshProUGUI text_now;
    public string[] log;
    bool inplace;
    bool opened;
    int now;
    void Start(){
        inplace = false;
        bk.SetActive(false);
        opened = false;
        now = 0;
    }
    private void Update(){
        if(Input.GetKeyDown("e")){
            if(inplace && !opened){
                bk.SetActive(true);
                opened = true;
            }
        }
        if(opened){
            text_now.text = log[now];
        }
        if(opened && Input.GetMouseButtonDown(0)){
            now ++;
            if(now == log.Length){
                now = 0;
                opened = false;
                text_now.text = "";
                bk.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = true;
            Player.instance.sign.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = false;
            Player.instance.sign.SetActive(false);
        }
    }
}
