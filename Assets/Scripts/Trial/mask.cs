using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mask : MonoBehaviour
{
    public Image Mask;
    float originalSize;
    void Start()
    {
        originalSize = Mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {				      
        Mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
