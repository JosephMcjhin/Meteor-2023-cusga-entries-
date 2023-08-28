using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    bool isopen;
    public GameObject trigger_object;
    bool inplace;
    void Start(){
        inplace = false;
        trigger_object.SetActive(false);
        isopen = false;
    }
    private void Update(){
        if(Input.GetKeyDown("e")){
            if(inplace){
                isopen = !isopen;
                trigger_object.SetActive(isopen);
                UIManager.instance.is_pause = isopen;
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
