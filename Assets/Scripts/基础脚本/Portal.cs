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
        Player.instance.sign.SetActive(false);
        if(Player.instance.main_sign == now_sign){
            Player.instance.transform.position = transform.position;
        }
    }

    private void Update(){
        if(Input.GetKeyDown("e")){
            if(inplace){
                TransManager.instance.Trans(to_scene, to_sign, true, false);
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
