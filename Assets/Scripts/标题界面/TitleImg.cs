using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleImg : MonoBehaviour
{
    Image temp;
    IEnumerator change(){
        for(float i=0;i<=1;i+=0.02f){
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, i/4f);
            yield return new WaitForSeconds(.02f);
        }
    }
    void Start(){
        temp = GetComponent<Image>();
        StartCoroutine(change());
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x - 5*Time.deltaTime, transform.position.y - 5*Time.deltaTime, transform.position.z);
        
    }
}
