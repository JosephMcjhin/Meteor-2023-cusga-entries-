using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boss_Dialog : MonoBehaviour
{
    public GameObject bk;
    public TextMeshProUGUI text_now;
    public string[] log;
    bool is_triggered;
    IEnumerator Display(){
        bk.SetActive(true);
        int temp = Random.Range(0,log.Length);
        text_now.text = log[temp];
        yield return new WaitForSeconds(1.5f);
        text_now.text = "";
        bk.SetActive(false);
        is_triggered = false;
    }

    public void Speak(){
        if(!is_triggered){
            is_triggered = true;
            StartCoroutine(Display());
        }
    } 
}
