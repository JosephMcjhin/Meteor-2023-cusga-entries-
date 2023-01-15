using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static TreeManager instance;
    void Awake(){
        if(instance!=null){
            Destroy(this);
        }
        instance = this;
    }

    public GameObject now_page;
    public GameObject now_text;

    public List<GameObject> total_page;
    public void Set_page(int x){
        if(now_page != null){
            now_page.SetActive(false);
        }
        now_page = total_page[x];
        now_page.SetActive(true);
        if(now_text != null){
            now_text.SetActive(false);
        }
    }

    public void Set_text(GameObject x){
        if(now_text != null){
            now_text.SetActive(false);
        }
        now_text = x;
        now_text.SetActive(true);
    }
}
