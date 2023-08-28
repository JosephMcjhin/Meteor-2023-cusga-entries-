using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject inventory;
    public GameObject talent;
    public GameObject notebook;
    public GameObject teleport;

    public bool is_pause;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            if(instance != this){
                Destroy(gameObject);
            }
        }
    }

    void Update(){  //设置按键开关UI，UI开启时游戏暂停
        if(Input.GetKeyDown("i")){
            bool temp = inventory.activeSelf;
            is_pause = !temp;
            inventory.SetActive(!temp);
        }
        if(Input.GetKeyDown("u")){
            bool temp = talent.activeSelf;
            is_pause = !temp;
            talent.SetActive(!temp);
            if(temp == false)TalentManager.instance.Refresh();
        }
        if(Input.GetKeyDown("n")){
            bool temp = notebook.activeSelf;
            is_pause = !temp;
            notebook.SetActive(!temp);
        }
        if(Input.GetKeyDown("r")){
            bool temp = teleport.activeSelf;
            is_pause = !temp;
            teleport.SetActive(!temp);
            if(temp == false)TeleportManager.instance.Refresh();
        }
    }
}
