using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleportDis : MonoBehaviour
{
    public TextMeshProUGUI tel_name;
    public TextMeshProUGUI tel_time;
    public string to_scene;
    public string to_sign;
    public int to_timeline;

    public void Tp(){
        Player.instance.now_timeline = to_timeline;
        TransManager.instance.now_timeline = to_timeline;
        TransManager.instance.Trans(to_scene, to_sign, true, true);
    }
}
