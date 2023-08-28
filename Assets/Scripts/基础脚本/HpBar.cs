using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image Mask_true;
    public Image Mask_false;
    Image now_bar;
    public float speed;
    public float start_size;
    float originalSize;
    float now_true;
    float now_false;
    void Start(){
        now_bar = gameObject.GetComponent<Image>();
        originalSize = Mask_true.rectTransform.rect.width;
        now_true = originalSize * start_size;
        now_false = originalSize * start_size;
    }

    public void SetValue(float value){				      
        now_true = originalSize * value;
        Mask_true.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, now_true);
    }

    void Update(){
        now_false = (now_false > now_true)?Mathf.Max(now_false - speed, now_true):now_true;
        Mask_false.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, now_false);
    }
}
