using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            if(instance != this){
                Destroy(gameObject);
            }
        }
    }
    public bool[] bursted = new bool[5];

    //武器加载切换及药水部分
    public float switch_cooldown;
    public float now_switch;
    public float potion_cooldown;
    public float now_potion;

    public bool switchable;

    int now_weapon;

    public NSlot[] weapon_slot;
    public NSlot[] potion_slot;
    public GameObject[] weapon_pos;

    public Sprite defaultimg;
    public Image[] weapon_img;
    public Image[] potion_img;
    public TextMeshProUGUI[] potion_num;

    public void Loadweapon(){
        for(int i = 0; i < 2; i ++){
            weapon_pos[i].SetActive(true);
            if(weapon_pos[i].transform.childCount != 0) Destroy(weapon_pos[i].transform.GetChild(0).gameObject);
            if(weapon_slot[i].SlotItem.Weapon != null){
                GameObject temp = Instantiate(weapon_slot[i].SlotItem.Weapon, weapon_pos[i].transform.position, Quaternion.identity);
                temp.transform. SetParent(weapon_pos[i].transform);
            }
            if(i != now_weapon)weapon_pos[i].SetActive(false);
        }

        LoadImage();
    }

    public void LoadImage(){
        for(int i = 0; i < 2; i ++){
            weapon_img[i].sprite = (weapon_slot[i].SlotNum1 == 0)?defaultimg:weapon_slot[i].SlotItem.ItemImage;
        }
        for(int i = 0; i < 3; i ++){
            potion_img[i].sprite = (potion_slot[i].SlotNum1 == 0)?defaultimg:potion_slot[i].SlotItem.ItemImage;
            potion_num[i].text = (potion_slot[i].SlotNum1 == 0)?"":potion_slot[i].SlotNum1.ToString();
        }
    }

    void Button(){
        if(Input.GetKeyDown("1")){
            if(now_weapon != 0 && now_switch <= 0 && switchable){
                now_weapon = 0;
                weapon_pos[1].SetActive(false);
                weapon_pos[0].SetActive(true);
                now_switch = switch_cooldown;
            }
        }
        if(Input.GetKeyDown("2")){
            if(now_weapon != 1 && now_switch <= 0 && switchable){
                now_weapon = 1;
                weapon_pos[0].SetActive(false);
                weapon_pos[1].SetActive(true);
                now_switch = switch_cooldown;
            }
        }
        if(Input.GetKeyDown("3")){
            if(now_potion <= 0 && NInventoryManager.instance.bag[2].NumList[potion_slot[0].SlotID] > 0){
                now_potion = potion_cooldown;
                //potion_slot[0].SlotItem.Weapon.Use();
                NInventoryManager.instance.bag[2].NumList[potion_slot[0].SlotID]--;
                NInventoryManager.instance.Refresh(potion_slot[0]);
                LoadImage();
            }
        }
        if(Input.GetKeyDown("4")){
            if(now_potion <= 0 && NInventoryManager.instance.bag[2].NumList[potion_slot[1].SlotID] > 0){
                now_potion = potion_cooldown;
                //potion_slot[1].SlotItem.Weapon.Use();
                NInventoryManager.instance.bag[2].NumList[potion_slot[1].SlotID]--;
                NInventoryManager.instance.Refresh(potion_slot[1]);
                LoadImage();
            }
        }
        if(Input.GetKeyDown("5")){
            if(now_potion <= 0 && NInventoryManager.instance.bag[2].NumList[potion_slot[2].SlotID] > 0){
                now_potion = potion_cooldown;
                //potion_slot[2].SlotItem.Weapon.Use();
                NInventoryManager.instance.bag[2].NumList[potion_slot[2].SlotID]--;
                NInventoryManager.instance.Refresh(potion_slot[2]);
                LoadImage();
            }
        }
    }


    //战斗开始结束的怪物生成，场景切换
    public bool[] enemy_dead = new bool[10];

    public int enemy_num;
    public List<EnemyWave> wave_list = new List<EnemyWave>();
    public string pre_scene;

    Vector3 pre_pos;

    public void Combat_start(string to_scene){
        enemy_num = 0;
        pre_pos = Player.instance.transform.position;
        TransManager.instance.Trans(to_scene, "", false, false);
    }

    public void Combat_end(){
        enemy_num = -1;
        TransManager.instance.Trans(pre_scene, "", false, false);
        Player.instance.transform.position = pre_pos;
    }

    //受伤计时
    public float total_time;
    public float last_hurt;
    public GameObject cloud;

    void Update(){
        now_switch = Mathf.Max(0, now_switch - Time.deltaTime);
        now_potion = Mathf.Max(0, now_potion - Time.deltaTime);
        Button();

        total_time += Time.deltaTime;
    }
    
    void Start(){
        enemy_num = -1;
        switchable = true;
    }
}
