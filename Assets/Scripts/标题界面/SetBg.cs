using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBg : MonoBehaviour
{
    void Start(){
        gameObject.GetComponent<Canvas>().worldCamera = Player.instance.now_camera;
    }
}
