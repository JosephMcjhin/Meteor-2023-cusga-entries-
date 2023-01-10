using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start_Game(){
        SceneManager.LoadScene("SampleScene");
        //Debug.Log("123");
    }

    public void Quit_Game(){
        Application.Quit();
        Debug.Log("123");
    }

    public void Close_Obj(GameObject a){
        a.SetActive(false);
    }
}
