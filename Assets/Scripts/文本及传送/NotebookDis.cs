using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NotebookDis : MonoBehaviour, IPointerClickHandler
{
    public Notebook notebook;
    public TextMeshProUGUI text_now;
    bool clicked;

    int now_page;

    public GameObject button1;
    public GameObject button2;

    int i;

    IEnumerator Display(){
        button1.SetActive(false);
        button2.SetActive(false);
        for(i = 0; i < notebook.note[now_page].text_con.Length; i ++){
            text_now.text = "";
            for(int j = 0; j < notebook.note[now_page].text_con[i].Length; j ++){
                text_now.text += notebook.note[now_page].text_con[i][j];
                if(clicked){
                    text_now.text = notebook.note[now_page].text_con[i];
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
        button1.SetActive(true);
        button2.SetActive(true);
    }
    
    public void Page_plus(){
        now_page ++;
        if(now_page == notebook.note.Count)now_page = 0;
        StartCoroutine(Display());
    }

    public void Page_minus(){
        now_page --;
        if(now_page == -1)now_page = notebook.note.Count - 1;
        StartCoroutine(Display());
    }

    public void OnPointerClick(PointerEventData eventData){
        Vector3 mouse_pos = new Vector3(eventData.position.x, eventData.position.y, 0);
        int pos_now = TMP_TextUtilities.FindIntersectingCharacter(text_now, mouse_pos, null, true);
        print(pos_now);
        for(int ii=0;ii<notebook.note[now_page].tel_loc.Length;ii++){
            if(notebook.note[now_page].tel_loc[ii] == i){
                if(notebook.note[now_page].tel_st[ii] <= pos_now && notebook.note[now_page].tel_ed[ii] >= pos_now){
                    TeleportManager.instance.Add_teleport(notebook.note[now_page].tel_id[ii]);
                    Player.instance.teleport_sign.SetActive(true);
                    return;
                }
            }
        }
    }
    

    void OnEnable(){
        text_now.text = "";
        if(notebook.note.Count == 0){
            button1.SetActive(false);
            button2.SetActive(false);
            return;
        }
        StartCoroutine(Display());
    }

    void Update(){
        if(Input.GetKeyDown("t")){
            clicked = true;
        }
    }
}