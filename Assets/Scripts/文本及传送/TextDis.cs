using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextDis : MonoBehaviour
{
    public Note raw_text;
    public TextMeshProUGUI text_now;
    bool clicked;

    IEnumerator Display(){
        for(int i = 0; i < raw_text.text_con.Length; i ++){
            text_now.text = "";
            for(int j = 0; j < raw_text.text_con[i].Length; j ++){
                text_now.text += raw_text.text_con[i][j];
                if(clicked){
                    text_now.text = raw_text.text_con[i];
                    //Debug.Log(123123);
                    clicked = false;
                    yield return null;
                    break;
                }
                yield return new WaitForSeconds(.05f);
            }
            while(true){
                if(clicked){
                    clicked = false;
                    yield return null;
                    break;
                }
                yield return null;
            }
        }
        //gameObject.SetActive(false);
    }
    /*
    public void OnPointerClick(PointerEventData eventData){
        Vector3 mouse_pos = new Vector3(eventData.position.x, eventData.position.y, 0);
        int pos_now = TMP_TextUtilities.FindIntersectingCharacter(text_now, mouse_pos, null, true);
        Debug.Log(pos_now);
    }
    */

    void OnEnable(){
        text_now.text = "";
        NotebookManager.instance.Add_text(raw_text);
        StartCoroutine(Display());
    }

    void Update(){
        if(Input.GetKeyDown("t")){
            clicked = true;
        }
    }
}