using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour
{
    public Image sign;
    public TextMeshProUGUI time_sign;
    public int flag;
    float height;

    void Start(){
        height = transform.parent.GetComponent<Image>().rectTransform.rect.width;
        print(height);
    }

    void Update(){
        if(flag == 1){
            sign.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height * CombatManager.instance.now_switch / CombatManager.instance.switch_cooldown);
            time_sign.text = (CombatManager.instance.now_switch<=0)?"":CombatManager.instance.now_switch.ToString("#0.0");
        }
        else{
            sign.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height * CombatManager.instance.now_potion / CombatManager.instance.potion_cooldown);
            time_sign.text = (CombatManager.instance.now_potion<=0)?"":CombatManager.instance.now_potion.ToString("#0.0");
        }
    }
}
