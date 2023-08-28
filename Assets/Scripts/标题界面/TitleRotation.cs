using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleRotation : MonoBehaviour
{
    Image temp;
    void Start(){
        temp = gameObject.GetComponent<Image>();
    }
    void Update(){
        transform.Rotate(0f,0f,Time.deltaTime*24f,Space.Self);
    }
}
