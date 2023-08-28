using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTrigger : MonoBehaviour
{
    public GameObject trigger_object;
    bool wait;
    void Start(){
        trigger_object.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player") && !wait){
            trigger_object.SetActive(true);
            wait = true;
        }
    }
}
