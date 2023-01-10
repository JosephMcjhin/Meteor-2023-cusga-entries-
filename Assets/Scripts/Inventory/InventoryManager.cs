using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;
    void Awake(){
        if(instance!=null){
            Destroy(this);
        }
        instance = this;
    }

    public Inventory bag;
    public GameObject Grid;
    public Slot SlotPrefab;

    private void OnEnable(){
        PlusHold();
    }

    public static void CreateItem(Item item){
        Slot slot = Instantiate(instance.SlotPrefab,instance.Grid.transform.position,Quaternion.identity);
        slot.gameObject.transform.SetParent(instance.Grid.transform);
        slot.SlotItem = item;
        slot.SlotImage.sprite = item.ItemImage;
        slot.SlotNum.text = item.ItemHold.ToString();
    }

    public static void PlusHold(){
        for(int i = 0; i < instance.Grid.transform.childCount;i++){
            Destroy(instance.Grid.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < instance.bag.ItemList.Count; i++){
            CreateItem(instance.bag.ItemList[i]);
        }
    }
}
