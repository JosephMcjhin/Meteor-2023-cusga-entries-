using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTable : MonoBehaviour
{
    public int[] tel_ids;
    public TeleportList tel_list;
    public GameObject tel_prefab;

    void Refresh(){
        for(int i=0;i<transform.childCount;i++){
            Destroy(transform.GetChild(i).gameObject);
        }
        for(int i=0;i<tel_ids.Length;i++){
            if(tel_list.unlocked[tel_ids[i]]){
                GameObject temp = Instantiate(tel_prefab, gameObject.transform.position, Quaternion.identity);
                temp.transform.SetParent(gameObject.transform);
                TeleportDis temp1 = temp.GetComponent<TeleportDis>();
                temp1.tel_time.text = tel_list.tel_list[tel_ids[i]].tel_time;
                temp1.tel_name.text = tel_list.tel_list[tel_ids[i]].tel_name;
                temp1.to_scene = tel_list.tel_list[tel_ids[i]].to_scene;
                temp1.to_sign = tel_list.tel_list[tel_ids[i]].to_sign;
                temp1.to_timeline = tel_ids[i];
            }
        }
    }

    void OnEnable(){
        Refresh();
    }
}
