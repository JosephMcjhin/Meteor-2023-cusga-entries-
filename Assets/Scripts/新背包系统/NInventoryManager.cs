using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NInventoryManager : MonoBehaviour
{
    public static NInventoryManager instance;
    void Awake(){
        if(instance!=null){
            Destroy(this);
        }
        instance = this;
    }
    public List<NInventory> bag = new List<NInventory>();
    public List<GameObject> grid = new List<GameObject>();
    public TextMeshProUGUI desc;
    public NItem defaultitem;

    void RefreshAll(){
        for(int i = 0; i < bag.Count; i ++){
            for(int j = 0; j < bag[i].max_capacity; j ++){
                NSlot temp = grid[i].transform.GetChild(j).GetChild(0).GetComponent<NSlot>();
                temp.SlotID = j;
                temp.SlotBelong = i;
                Refresh(temp);
            }
        }

        CombatManager.instance.Loadweapon();
    }

    void Start(){
        RefreshAll();
        SetDesc("这里是物品描述");
    }

    void OnEnable(){
        transform.position = transform.parent.position;
        SetDesc("这里是物品描述");
    }

    public void Refresh(NSlot x){
        NItem temp1 = instance.bag[x.SlotBelong].ItemList[x.SlotID];
        int temp2 = instance.bag[x.SlotBelong].NumList[x.SlotID];
        if(temp2 == 0){
            instance.bag[x.SlotBelong].ItemList[x.SlotID] = defaultitem;
            temp1 = defaultitem;
        }
        x.SlotItem = temp1;
        x.SlotImage.sprite = temp1.ItemImage;
        x.SlotNum1 = temp2;
        x.SlotNum.text = (x.SlotNum1 == 0 || x.SlotBelong == 1)?"":x.SlotNum1.ToString();
    }

    public void SwapList(int inv_id1, int slot_id1, int inv_id2, int slot_id2){ 
        NInventory temp11 = instance.bag[inv_id1];
        NInventory temp22 = instance.bag[inv_id2];

        NItem temp = temp11.ItemList[slot_id1];
        temp11.ItemList[slot_id1] = temp22.ItemList[slot_id2];
        temp22.ItemList[slot_id2] = temp;
        int temp1 = temp11.NumList[slot_id1];
        temp11.NumList[slot_id1] = temp22.NumList[slot_id2];
        temp22.NumList[slot_id2] = temp1;
    }

    public void SetDesc(string x){
        desc.text = x;
    }
}
