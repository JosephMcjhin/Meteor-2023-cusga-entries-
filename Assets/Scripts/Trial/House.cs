using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class House : MonoBehaviour
{
    // Start is called before the first frame update
    public string to_scene;
    private void OnTriggerStay2D(Collider2D other){
        Debug.Log("123");
        if(Input.GetKeyDown("e")){
            //Debug.Log("123");
            if(other.gameObject.CompareTag("Player")){
                SceneManager.LoadScene(to_scene);
            }
        }
    }
}
