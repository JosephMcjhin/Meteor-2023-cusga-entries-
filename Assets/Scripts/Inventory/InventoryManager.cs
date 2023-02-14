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
    public List<Inventory> Bag = new List<Inventory>();
    public List<GameObject> Grid = new List<GameObject>();
    //public Slot SlotPrefab;
    public Item DefaultItem;

    //public Inventory weapon_bag;
    //public GameObject weapon_grid;

    //public Inventory merge_bag;
    //public GameObject merge_grid;

    public bool isok;

    public Merge mm;

    Dictionary<string, Item> dic = new Dictionary<string, Item>();

    void Start(){
        for(int i = 0;i < mm.recipe.Count; i ++){
            dic[mm.recipe[i]] = mm.product[i];
        }
    }

    private void OnEnable(){
        transform.GetChild(0).position = transform.position;
        isok = true;
        for(int i = 0; i < Bag.Count; i ++){
            Refresh(instance.Bag[i], instance.Grid[i]);
        }
    }

    public static void pickup_add(Item item, int index){
        AddItem(item, index, instance.Grid[0], instance.Bag[0]);
    }

    public static void AddItem(Item item, int index, GameObject Grid, Inventory bag){
        Slot temp = Grid.transform.GetChild(index).gameObject.transform.GetChild(0).GetComponent<Slot>();
        temp.SlotItem = item;
        temp.SlotImage.sprite = item.ItemImage;
        temp.SlotNum.text = bag.NumList[index].ToString();
        temp.SlotDes.text = item.ItemInfo;
        if(bag.NumList[index] == 0 || Grid.transform.GetChild(index).gameObject.CompareTag("Weapon") || Grid.transform.GetChild(index).gameObject.CompareTag("Merge")){
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
        var temp11 = instance.Bag[from1-1];
        var temp22 = instance.Bag[from2-1];
        var temp33 = instance.Grid[from1-1];
        var temp44 = instance.Grid[from2-1];

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
            Tank.instance.Weapon = instance.Bag[1].ItemList[0].Weapon;
            if(instance.Bag[1].ItemList[0].Weapon != null){
                GameObject new_Weapon = Instantiate(instance.Bag[1].ItemList[0].Weapon,Tank.instance.Weapon_slot.transform.position,Quaternion.identity);
                new_Weapon.transform.SetParent(Tank.instance.Weapon_slot.transform);
            }
        }

        AddItem(temp11.ItemList[x1],x1,temp33,temp11);
        AddItem(temp22.ItemList[x2],x2,temp44,temp22);
    }

    public static void DelPre(){
        instance.Bag[2].ItemList[0] = instance.Bag[2].ItemList[1] = instance.DefaultItem;
        instance.Bag[2].NumList[0] = instance.Bag[2].NumList[1] = 0;
        AddItem(instance.DefaultItem, 0, instance.Grid[2], instance.Bag[2]);
        AddItem(instance.DefaultItem, 1, instance.Grid[2], instance.Bag[2]);
    }

    void Update(){
        if(Bag[2].ItemList[2].ItemID == "0000" && dic.ContainsKey(Bag[2].ItemList[0].ItemID + Bag[2].ItemList[1].ItemID)){
            Bag[2].ItemList[2] = dic[Bag[2].ItemList[0].ItemID + Bag[2].ItemList[1].ItemID];
            Bag[2].NumList[2] = 1;
            AddItem(Bag[2].ItemList[2], 2, Grid[2], Bag[2]);
        }
    }
}
