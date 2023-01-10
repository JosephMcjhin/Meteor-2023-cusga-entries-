using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    public Inventory bag;

    private void OnCollisionStay2D(Collision2D other){
        if(Input.GetKeyDown("e")){
            //Debug.Log("123");
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
