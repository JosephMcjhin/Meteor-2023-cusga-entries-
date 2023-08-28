using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleAction : MonoBehaviour
{
    public GameObject title;
    public GameObject bg;
    public Note[] raw_text;
    public TextMeshProUGUI text_now;
    public GameObject[] text_img;

    bool clicked;
    
    IEnumerator Display(){
        for(int ii=0;ii<raw_text.Length;ii++){
            clicked = false;
            text_now.text = "";
            text_now.color = new Color(text_now.color.r, text_now.color.g, text_now.color.b, 1f);
            text_img[ii].SetActive(true);
            if(clicked)continue;
            for(int i = 0; i < raw_text[ii].text_con.Length; i ++){
                for(int j = 0; j < raw_text[ii].text_con[i].Length; j ++){
                    text_now.text += raw_text[ii].text_con[i][j];
                    if(clicked)break;
                    yield return new WaitForSeconds(.05f);
                }
                if(clicked)break;
                text_now.text += "\n";
                yield return new WaitForSeconds(1f);
            }
            if(clicked)continue;
            yield return new WaitForSeconds(3f);
            Image temp = text_img[ii].GetComponent<Image>();
            for(float i=1;i>=0;i-=0.02f){
                text_now.color = new Color(text_now.color.r, text_now.color.g, text_now.color.b, i);
                temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, i/4f);
                if(clicked)break;
                yield return new WaitForSeconds(.02f);
            }
            if(clicked)continue;
            text_img[ii].SetActive(false);
            yield return new WaitForSeconds(3f);
        }
        TransManager.instance.StartTrans();
    }

    public void Start_game(){
        title.SetActive(false);
        bg.SetActive(true);
        StartCoroutine(Display());
    }

    public void Quit_game(){
        Application.Quit();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            clicked = true;
        }
    }
}
