using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    bool isopen;
    public GameObject text;
    bool inplace;
    void Start(){
        isopen = false;
        text.SetActive(isopen);
        inplace = false;
    }
    private void Update(){
        if(Input.GetKeyDown("e")){
            if(inplace){
                isopen = !isopen;
                text.SetActive(isopen);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = true;
            Tank.instance.sign.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = false;
            Tank.instance.sign.SetActive(false);
        }
    }
}
