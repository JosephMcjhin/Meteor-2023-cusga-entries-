using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject Flower_Prefab;
    private void OnCollisionStay2D(Collision2D other){
        Debug.Log("123");
        if(Input.GetKeyDown("e")){
            //Debug.Log("123");
            if(other.gameObject.CompareTag("Player")){
                Flower_Gen();
            }
        }
    }
    void Flower_Gen(){
        Vector2 temp;
        temp.x = transform.position.x;
        temp.y = transform.position.y-5;
        GameObject newf = Instantiate(Flower_Prefab, temp, Quaternion.identity);
    }
}
