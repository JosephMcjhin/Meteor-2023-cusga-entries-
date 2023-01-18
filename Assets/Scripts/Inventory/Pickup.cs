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
                if(bag.ItemList.Contains(item)){
                    item.ItemHold ++;
                    InventoryManager.PlusHold();
                }
                else{
                    bag.ItemList.Add(item);
                    InventoryManager.CreateItem(item);
                    item.ItemHold = 1;
                }
                Destroy(gameObject);
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

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            if(bag.ItemList.Contains(item)){
                item.ItemHold ++;
                InventoryManager.PlusHold();
            }
            else{
                bag.ItemList.Add(item);
                InventoryManager.CreateItem(item);
                item.ItemHold = 1;
            }
            Destroy(gameObject);
        }
    }
}
