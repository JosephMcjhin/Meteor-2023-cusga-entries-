using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    public string to_scene;
    public string now_sign;
    public string to_sign;
    bool inplace;
    
    void Start(){
        inplace = false;
        Tank.instance.sign.SetActive(false);
        if(Tank.instance.main_sign == now_sign){
            Tank.instance.transform.position = transform.position;
        }
    }

    private void Update(){
        if(Input.GetKeyDown("e")){
            if(inplace){
                Tank.instance.main_sign = to_sign;
                SceneManager.LoadScene(to_scene);
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
