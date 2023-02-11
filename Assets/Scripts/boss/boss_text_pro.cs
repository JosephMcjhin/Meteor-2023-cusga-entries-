using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class boss_text_pro : MonoBehaviour
{
    [TextArea(1,5)]
    public string[] text_con;
    public TextMeshProUGUI text_x;
    public GameObject background;

    bool flag;

    IEnumerator Xianshi(){
        for(int i = 0; i < text_con.Length; i ++){
            text_x.text = "";
            for(int j = 0; j < text_con[i].Length; j ++){
                text_x.text += text_con[i][j];
                if(flag){
                    text_x.text = text_con[i];
                    Debug.Log(123123);
                    flag = false;
                    yield return null;
                    break;
                }
                yield return new WaitForSeconds(.05f);
            }
            while(true){
                if(flag){
                    flag = false;
                    yield return null;
                    break;
                }
                yield return null;
            }
        }
        background.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable(){
        background.SetActive(true);
        StartCoroutine("Xianshi");
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            flag = true;
        }
    }
}
