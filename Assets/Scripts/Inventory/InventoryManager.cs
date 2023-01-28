using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    void Awake(){
        if(instance!=null){
            Destroy(this);
        }
        instance = this;
    }
    void Start(){
        Refresh(bag, Grid);
        Refresh(weapon_bag, weapon_grid);
    }
    public Inventory bag;
    public GameObject Grid;
    //public Slot SlotPrefab;
    public Item DefaultItem;

    public Inventory weapon_bag;
    public GameObject weapon_grid;

    private void OnEnable(){
        Refresh(bag, Grid);
        Refresh(weapon_bag, weapon_grid);
    }

    public static void pickup_add(Item item, int index){
        AddItem(item, index, instance.Grid, instance.bag);
    }

    public static void AddItem(Item item, int index, GameObject Grid, Inventory bag){
        Slot temp = Grid.transform.GetChild(index).gameObject.transform.GetChild(0).GetComponent<Slot>();
        temp.SlotItem = item;
        temp.SlotImage.sprite = item.ItemImage;
        temp.SlotNum.text = bag.NumList[index].ToString();
        if(bag.NumList[index] == 0 || Grid.transform.GetChild(index).gameObject.CompareTag("Weapon")){
            temp.SlotNum.enabled = false;
        }
        else{
            temp.SlotNum.enabled = true;
        }
        //Slot slot = Instantiate(instance.SlotPrefab,instance.Grid.transform.position,Quaternion.identity);
        //slot.gameObject.transform.SetParent(instance.Grid.transform);
        //slot.SlotItem = item;
        //slot.SlotImage.sprite = item.ItemImage;
        //slot.SlotNum.text = item.ItemHold.ToString();
    }

    public static void Refresh(Inventory bag, GameObject Grid){
        if(bag.ItemList.Count < bag.max_capacity){
            int temp = bag.max_capacity - bag.ItemList.Count;
            for(int i = 0; i < temp; i++){
                bag.ItemList.Add(instance.DefaultItem);
                bag.NumList.Add(0);
            }
        }
        for(int i = 0; i < bag.ItemList.Count; i++){
            AddItem(bag.ItemList[i], i, Grid, bag);
        }
    }

    public static void SwapList(int x1,int from1,int x2,int from2){
        var temp11 = from1==1?instance.bag:instance.weapon_bag;
        var temp22 = from2==1?instance.bag:instance.weapon_bag;
        var temp33 = from1==1?instance.Grid:instance.weapon_grid;
        var temp44 = from2==1?instance.Grid:instance.weapon_grid;

        var temp = temp11.ItemList[x1];
        temp11.ItemList[x1] = temp22.ItemList[x2];
        temp22.ItemList[x2] = temp;
        var temp1 = temp11.NumList[x1];
        temp11.NumList[x1] = temp22.NumList[x2];
        temp22.NumList[x2] = temp1;

        if((from1 == 2 && x1 == 0) || (from2 == 2 && x2 == 0)){     //武器栏变化时，将武器的对象创建或者销毁
            if(Tank.instance.Weapon_slot.transform.childCount != 0){
                //Debug.Log(Tank.instance.Weapon_slot.transform.childCount);
                Destroy(Tank.instance.Weapon_slot.transform.GetChild(0).gameObject);
            }
            Tank.instance.Weapon = instance.weapon_bag.ItemList[0].Weapon;
            if(instance.weapon_bag.ItemList[0].Weapon != null){
                GameObject new_Weapon = Instantiate(instance.weapon_bag.ItemList[0].Weapon,Tank.instance.Weapon_slot.transform.position,Quaternion.identity);
                new_Weapon.transform.SetParent(Tank.instance.Weapon_slot.transform);
            }
        }

        AddItem(temp11.ItemList[x1],x1,temp33,temp11);
        AddItem(temp22.ItemList[x2],x2,temp44,temp22);
    }
}
