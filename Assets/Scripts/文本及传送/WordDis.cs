using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordDis : MonoBehaviour
{
    public Note raw_text;
    public TextMeshProUGUI text_now;
    public Image bg;

    IEnumerator Display(){
        for(float i=0;i<=100f/255f;i+=0.05f){
            bg.color = new Color(bg.color.r,bg.color.g,bg.color.b,i);
            yield return new WaitForSeconds(.02f);
        }
        for(int i = 0; i < raw_text.text_con.Length; i ++){
            text_now.text = "";
            for(int j = 0; j < raw_text.text_con[i].Length; j ++){
                text_now.text += raw_text.text_con[i][j];
                yield return new WaitForSeconds(.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(2f);
        for(float i=1;i>=0;i-=0.02f){
            text_now.color = new Color(text_now.color.r, text_now.color.g, text_now.color.b, i);
            yield return new WaitForSeconds(.02f);
        }
        for(float i=100f/255f;i>=0;i-=0.05f){
            bg.color = new Color(bg.color.r,bg.color.g,bg.color.b,i);
            yield return new WaitForSeconds(.02f);
        }
        Destroy(transform.parent.gameObject);
        //gameObject.SetActive(false);
    }

    void OnEnable(){
        text_now.text = "";
        StartCoroutine(Display());
    }
}
