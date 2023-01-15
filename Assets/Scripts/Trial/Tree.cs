using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject Flower_Prefab;
    bool inplace;
    void Start(){
        inplace = false;
    }
    void Flower_Gen(){
        Vector2 temp;
        temp.x = transform.position.x;
        temp.y = transform.position.y-5;
        GameObject newf = Instantiate(Flower_Prefab, temp, Quaternion.identity);
    }
    private void Update(){
        if(Input.GetKeyDown("e")){
            if(inplace){
                Flower_Gen();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = true;
            Tank.instance.sign.SetActive(true);
        }
    }
    private void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = false;
            Tank.instance.sign.SetActive(false);
        }
    }
}
