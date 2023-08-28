using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager instance;
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

    public TeleportList tel_list;
    //public GameObject time_line;
    public GameObject[] tel_dis;
    public TextMeshProUGUI desc;

    public void Refresh(){
        desc.text = "";
        for(int i=0;i<tel_list.unlocked.Length;i++){
            if(tel_list.unlocked[i]){
                tel_dis[i].SetActive(true);
            }
            else{
                tel_dis[i].SetActive(false);
            }
        }
    }

    public void Add_teleport(int x){
        if(!tel_list.unlocked[x]){
            tel_list.unlocked[x] = true;
            //Refresh();
        }
    }

    public void Display(int x){
        desc.text = tel_list.tel_list[x].tel_info;
    }

}
