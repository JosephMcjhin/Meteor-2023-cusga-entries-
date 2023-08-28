using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Passive_Dialog : MonoBehaviour
{
    public GameObject bk;
    public TextMeshProUGUI text_now;
    public string log;
    bool is_triggered;
    IEnumerator Display(){
        bk.SetActive(true);
        text_now.text = log;
        yield return new WaitForSeconds(3f);
        text_now.text = "";
        bk.SetActive(false);
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D other){
        Player a = other.gameObject.GetComponent<Player>();
        if(a != null && !is_triggered){
            StartCoroutine(Display());
            is_triggered = true;
        }
    } 
}
