using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEff : MonoBehaviour
{
    public SpriteRenderer temp;
    public int state;
    public void init(int x){
        state = x;
        if(state == 5){
            temp.color = new Color(1f,1f,1f,1f);
        }
        else if(state == 0){
            temp.color = new Color(1f,1f,1f,0.5f);
        }
        else if(state == 1){
            temp.color = new Color(0f,1f,0f,1f);
        }
        else if(state == 2){
            temp.color = new Color(1f,1f,0f,.5f);
        }
        else if(state == 3){
            temp.color = new Color(1f,0f,1f,.5f);
        }
        else if(state == 4){
            temp.color = new Color(0f,0f,0f,1f);
        }
    }
    void Update()
    {
        if(Player.instance.now_shield <= 0){
            gameObject.SetActive(false);
        }
    }
}
