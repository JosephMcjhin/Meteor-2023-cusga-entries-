using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Damage : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float force;
    public TextMeshProUGUI da;
    public int damage;
    // Start is called before the first frame update
    IEnumerator Fade(){
        for (float f = 1f; f >= 0; f -= 0.1f) {
            da.color = new Color(da.color.r,da.color.g,da.color.b,f);
            yield return new WaitForSeconds(.1f);
        }
        Destroy(gameObject);
    }

    void Start(){
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(0,force);
        StartCoroutine(Fade());
        if(damage >= 0)da.color = new Color(0f,1f,0f,da.color.a);
        else da.color = new Color(1f,0f,0f,da.color.a);
        da.text = damage.ToString();
    }

}
