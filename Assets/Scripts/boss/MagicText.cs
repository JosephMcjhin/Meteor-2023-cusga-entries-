using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MagicText : MonoBehaviour
{
    public Note[] raw_text = new Note[3];
    public TextMeshProUGUI[] text_now = new TextMeshProUGUI[3];
    public Image bg;
    int finished;

    IEnumerator Display(int now){
        //print(123123);
        for(int i = 0; i < raw_text[now].text_con.Length; i ++){
            text_now[now].text = "";
            for(int j = 0; j < raw_text[now].text_con[i].Length; j ++){
                text_now[now].text += raw_text[now].text_con[i][j];
                yield return new WaitForSeconds(.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        for(float i=1;i>=0;i-=0.02f){
            text_now[now].color = new Color(text_now[now].color.r, text_now[now].color.g, text_now[now].color.b, i);
            yield return new WaitForSeconds(.02f);
        }
        finished ++;
    }

    IEnumerator Dis(){
        for(float i=0;i<=100f/255f;i+=0.05f){
            bg.color = new Color(bg.color.r,bg.color.g,bg.color.b,i);
            yield return new WaitForSeconds(.02f);
        }
        for(int i=0;i<3;i++){
            if(raw_text[i]!=null){
                StartCoroutine(Display(i));
                yield return new WaitForSeconds(2f);
            }
            else{
                finished ++;
            }
        }
        while(finished != 3){
            yield return null;
        }
        for(float i=100f/255f;i>=0;i-=0.05f){
            bg.color = new Color(bg.color.r,bg.color.g,bg.color.b,i);
            yield return new WaitForSeconds(.02f);
        }
        Destroy(transform.parent.gameObject);
    }
    void OnEnable(){
        StartCoroutine(Dis());
    }
}

