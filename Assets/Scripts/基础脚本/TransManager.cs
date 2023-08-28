using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TransManager : MonoBehaviour
{
    public static TransManager instance;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            if(instance != this){
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
    public int now_timeline;
    public GameObject scene_trans;
    public RectTransform circle;
    public TeleportList tel_list;
    public TextMeshProUGUI time_sign;

    IEnumerator trans(string to_scene, bool dis_sign){
        scene_trans.SetActive(true);
        for(float i = 0.5f; i <= 10; i += 0.4f){
            circle.localScale = new Vector3(i, i, circle.localScale.z);
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene(to_scene);
        if(!dis_sign){
            Player.instance.sign.SetActive(false);
            TalentManager.instance.QinheClear();
        }
        if(dis_sign){
            time_sign.text = tel_list.tel_list[now_timeline].tel_time;
            yield return new WaitForSeconds(3f);
            time_sign.text = "";
        }
        for(float i = 10; i >= 0.5f; i -= 0.4f){
            circle.localScale = new Vector3(i, i, circle.localScale.z);
            yield return new WaitForSeconds(0.02f);
        }
        scene_trans.SetActive(false);
    }

    public void Trans(string to_scene, string to_sign, bool reset, bool dis_sign){
        if(reset)CombatManager.instance.enemy_dead = new bool[10];
        Player.instance.main_sign = to_sign;
        StartCoroutine(trans(to_scene,dis_sign));
    }

    public void StartTrans(){
        StartCoroutine(trans("进入山谷", true));
    }

    void Start(){
        scene_trans.SetActive(false);
    }
}
