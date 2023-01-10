using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    bool isopen;
    public GameObject text;
    void Start(){
        isopen = false;
        text.SetActive(isopen);
    }
    private void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            if(Input.GetKeyDown("e")){
                isopen = !isopen;
                Debug.Log("123");
                text.SetActive(isopen);
            }
        }
    }
}
