using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    public Inventory bag;
    bool inplace;

    void Start(){
        inplace = false;
        Tank.instance.sign.SetActive(false);
    }

    private void Update(){
        if(Input.GetKeyDown("e")){
            if(inplace){
                int index = bag.ItemList.IndexOf(item);
                if(index != -1){
                    bag.NumList[index]++;
                    InventoryManager.pickup_add(item, index);
                    Destroy(gameObject);
                }
                else{
                    for(int i = 0; i < bag.ItemList.Count; i++){
                        if(bag.NumList[i] == 0){
                            bag.NumList[i] ++;
                            bag.ItemList[i] = item;
                            InventoryManager.pickup_add(item, i);
                            Destroy(gameObject);
                            break;
                        }
                    }
                    //item.ItemHold = 1;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = true;
            Tank.instance.sign.SetActive(true);
        }
    }
    private void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            inplace = false;
            Tank.instance.sign.SetActive(false);
        }
    }

}
