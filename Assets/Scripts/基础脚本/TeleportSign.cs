using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleportSign : MonoBehaviour
{
    public Image temp1;
    public TextMeshProUGUI temp2;

    IEnumerator Fade(){
        yield return new WaitForSeconds(2f);
        for(float i=1;i>=0;i-=0.02f){
            temp2.color = new Color(temp2.color.r, temp2.color.g, temp2.color.b, i);
            temp1.color = new Color(temp1.color.r, temp1.color.g, temp1.color.b, i);
            yield return new WaitForSeconds(.02f);
        }
        temp2.color = new Color(temp2.color.r, temp2.color.g, temp2.color.b, 1);
        temp1.color = new Color(temp1.color.r, temp1.color.g, temp1.color.b, 1);
        gameObject.SetActive(false);
    }

    void OnEnable(){
        StartCoroutine(Fade());
    }
}
