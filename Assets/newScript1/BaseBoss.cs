using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseBoss : MonoBehaviour
{
    public Animator animator;
    //public GameObject player;
    public List<GameObject> child_boss_list = new List<GameObject>();
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer sprite_render;

    public bool is_boss;


    // base properties
    protected int boss_id = 0;
    public int base_health_per_level;
    protected int max_health;
    protected int now_health = 0;
    public float move_speed = 0;
    protected int boss_level = 1;
    protected Vector2 move_dir;
    protected Vector2 current_position;
    public float prepare_distance;    


    // attack properties
    public GameObject[] weapons;
    protected int weapon_id = 0;
    public float change_weapon_time_interval;
    protected float change_weapon_time_count = 0;

    public int base_attack_value = 0;
    protected int attack_value = 0;


    // defense properties
    public GameObject[] shields;
    protected int shield_id = 0;
    public float change_shield_time_interval;
    protected float change_shield_time_count = 0;

    protected int[] element_defense_value = new int[5] {0, 0, 0, 0, 0};
    public int max_element_defense_value = 0;
    protected int cloud_defense_value = 0;
    protected int stone_defense_value = 0;
    protected int sand_defense_value = 0;
    protected int snow_defense_value = 0;
    protected int iron_defense_value = 0;
    public int base_defense_value = 0;
    protected int defense_value = 0;
    protected int attacked_number = 0;

    protected bool is_hurt = false;


    // boss state
    protected bool boss_initial = false;
    protected bool boss_die = false;
    protected int boss_state = 0;

    public float hidden_time;
    protected float hidden_time_count = 0;
    protected bool hidden = false;
    protected bool beatable = true;
    public float unbeatable_time_interval = 0f;
    protected float unbeatable_time_count = 0f;


    public GameObject damage_prefab;
    public int weapon_invisible;
    protected int now_weapon_invisible;
    public HpBar mm;
    public Image[] charge_bar = new Image[5];
    public float[] charge_now = new float[5];
    public string enemy_name;
    public TextMeshProUGUI name_text;

    protected Vector2 rotate(Vector2 now, float angle)
    {
        Vector2 temp;
        angle = angle * Mathf.PI / 180;
        temp.x = now.x * Mathf.Cos(angle) + now.y * Mathf.Sin(angle);
        temp.y = -now.x * Mathf.Sin(angle) + now.y * Mathf.Cos(angle);
        return temp;
    }

    IEnumerator Hurt(){
        sprite_render.material.SetFloat("_FlashAmount", 0.8f);
        yield return new WaitForSeconds(.2f);
        sprite_render.material.SetFloat("_FlashAmount", 0);
    }
    public void ChangeHealth(int x, bool judge){
        /*
        if (beatable)
        {
            animator.SetBool("isHurt", true);
            is_hurt = true;
            now_health += delta;
            beatable = false;
        }
        attacked_number += 1;
        */
        if(!beatable)return;
        if(judge && now_weapon_invisible > 0)return;
        now_health += x;
        mm.SetValue((float)now_health/(float)max_health);
        float temp1 = Random.Range(-1f,1f);
        float temp2 = Random.Range(-1f,1f);
        GameObject temp = Instantiate(damage_prefab, new Vector2(temp1+transform.position.x,temp2+transform.position.y), Quaternion.identity);
        temp.GetComponent<Damage>().damage = x;
        StartCoroutine(Hurt());
        if (now_health <= 0){
            CombatManager.instance.enemy_num--;
            for(int i=0;i<child_boss_list.Count;i++){
                if(child_boss_list[i]!=null)
                Destroy(child_boss_list[i]);
            }
            Destroy(gameObject);
        }
    }
    void ChargeSet(){
        if(!is_boss){
            for(int i=0;i<5;i++){
                charge_bar[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.5f * charge_now[i]);
            }
        }
        else{
            for(int i=0;i<5;i++){
                charge_bar[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140f * charge_now[i]);
            }
        }
    }
    public void Init(int x){
        boss_level = x;
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        sprite_render = GetComponent<SpriteRenderer>();
        max_health = (int)(base_health_per_level * boss_level/3f);
        attack_value = (int)(base_attack_value * boss_level/3f);
        defense_value = (int)(base_defense_value * boss_level/3f);
        charge_now = new float[5]; 
        ChargeSet();
        name_text.text = "LV" + boss_level.ToString() + " " + enemy_name;
    }
    public void ChargeEn(float[] charge, bool judge){
        if(!beatable)return;
        if(judge && now_weapon_invisible > 0)return;
        for(int i=0;i<5;i++){
            charge_now[i] += charge[i];
            charge_now[i] = Mathf.Min(1f,charge_now[i]);
        }
        if(judge){
            now_weapon_invisible = weapon_invisible;
        }
        ChargeSet();
    }
    public void ChargeClear(){
        charge_now = new float[5];
        ChargeSet();
    }
    public void ChangePos(Vector2 delta){
        transform.position = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y, transform.position.z);
    }


    public int GetElement(int element_id)
    {
        if (element_id >= element_defense_value.Length){
            Debug.LogError("element id out of range.");
            return -1;
        }
        return element_defense_value[element_id];
    }

    public void ChangeElement(int element_id, int delta)
    {
        if (element_id >= element_defense_value.Length){
            Debug.LogError("element id out of range.");
            return;
        }
        element_defense_value[element_id] += delta;
    }
    

    public int GetBossId()
    {
        return boss_id;
    }
    public void SetBossId(int new_boss_id)
    {
        boss_id = new_boss_id;
    }


    public int GetBossState()
    {
        return boss_state;
    }
    public void SetBossState(int new_state)
    {
        boss_state = new_state;
    }


    public int GetHealth()
    {
        return now_health;
    }
    public int GetAndSetHealth(int new_health)
    {
        if (0 > new_health || max_health < new_health)
        {
            Debug.LogError("Health need to be in the legal range.");
            return -1;
        }
        now_health = new_health;
        return now_health;
    }
    public void ChangeMaxHealth(int new_max_health){
        max_health = new_max_health;
    }

    public int GetAttackValue()
    {
        return attack_value;
    }
    public void ChangeAttackValue(int delta)
    {
        attack_value += delta;
    }
    public int GetAndSetAttackValue(int new_attack_value)
    {
        attack_value = new_attack_value;
        return attack_value;
    }

    public int GetDefenseValue()
    {
        return defense_value;
    }
    public void ChangeDefenseValue(int delta)
    {
        defense_value += delta;
    }
    public int GetAndSetDefenseValue(int new_defense_value)
    {
        defense_value = new_defense_value;
        return defense_value;
    }



    public int GetBossLevel()
    {
        return boss_level;
    }
    public int SetBossLevel(int new_boss_level)
    {
        if (0 > new_boss_level)
        {
            Debug.LogError("Boss level must > 0");
            return -1;
        }
        boss_level = new_boss_level;
        // boss level变化，将引起boss生命，基础攻击力，基础防御力变化
        UpdateBossLife(boss_level);
        UpdateBossAttackValue(boss_level);
        UpdateBossDenfenseValue(boss_level);
        return boss_level;
    }


    public void SetInitial(bool if_initial)
    {
        boss_initial = if_initial;
    }


    protected Vector2 Judge4Direction()
    {
        Vector2 direction = new Vector2(0, 0);
        if (0 >= move_dir.x + Mathf.Abs(move_dir.y))        direction.Set(-1, 0);//左 3
        else if (0 > -move_dir.x + Mathf.Abs(move_dir.y))   direction.Set(1, 0);//右 1
        else if (0 >= Mathf.Abs(move_dir.x) + move_dir.y)   direction.Set(0, -1);//下 0
        else                                                direction.Set(0, 1);//上 2
        return direction;
    }

    protected Vector2 Judge4Direction(Vector2 dir)
    {
        Vector2 direction = new Vector2(0, 0);
        if (0 >= dir.x + Mathf.Abs(dir.y))        direction.Set(-1, 0);
        else if (0 > -dir.x + Mathf.Abs(dir.y))   direction.Set(1, 0);
        else if (0 >= Mathf.Abs(dir.x) + dir.y)   direction.Set(0, -1);
        else                                      direction.Set(0, 1);
        return direction;
    }

    protected Vector2 Judge2Direction()
    {
        Vector2 direction = new Vector2(0, 0);
        if (move_dir.x >= 0)    direction.Set(1, 0);
        else                    direction.Set(-1, 0);
        return direction;
    }
    protected Vector2 Judge2Direction(Vector2 dir)
    {
        Vector2 direction = new Vector2(0, 0);
        if (dir.x >= 0)     direction.Set(1, 0);
        else                direction.Set(-1, 0);
        return direction;
    }

    protected virtual void SwitchWeapon()
    {
        bool change_weapon = (change_weapon_time_interval <= change_weapon_time_count);
        if (change_weapon) {
            weapons[weapon_id].SetActive(false);
            // Debug.Log("current weapon id " + weapon_id.ToString());
            // weapon_id < 0时，循环到weapon list最后一个武器
            if (--weapon_id < 0) 
                weapon_id = weapons.Length - 1;

            weapons[weapon_id].SetActive(true);
            Debug.Log("current activate weapon id " + weapon_id.ToString());
            change_weapon_time_count = 0;
        }
        else
            change_weapon_time_count += Time.deltaTime;
    }


    protected void SwitchShield()
    {
        bool change_shield = (change_shield_time_interval <= change_shield_time_count);
        if (change_shield) {
            shields[shield_id].SetActive(false);
            // shield_id < 0时，循环到shield list最后一个护盾
            if (--shield_id < 0) 
                shield_id = shields.Length - 1;

            shields[shield_id].SetActive(true);
            change_shield_time_count = 0;
        }
        else
            change_shield_time_count += Time.deltaTime;
    }
    


    void UpdateBossLife(int current_boss_level)
    {
        max_health = base_health_per_level * current_boss_level;
    }

    void UpdateBossAttackValue(int current_boss_level)
    {
        attack_value = base_attack_value * current_boss_level;
    }

    void UpdateBossDenfenseValue(int current_boss_level)
    {
        defense_value = base_defense_value * current_boss_level;
    }



    public void Move(float speed)
    {
        animator.SetBool("isMoving", true);

        move_dir = move_dir.normalized;
        current_position.x = current_position.x + speed * move_dir.x * Time.deltaTime;
        current_position.y = current_position.y + speed * move_dir.y * Time.deltaTime;
        rigidbody2d.MovePosition(current_position);
    }

    public void RandomMove(float speed) {
        move_dir.x += Random.Range(-5f, 5f);
        move_dir.y += Random.Range(-2.5f, 2.5f);
        Move(speed);
    }

    protected void ClearDirection(){
        move_dir.x = 0;
        move_dir.y = 0;
    }

    protected void CalculateDirection()
    {
        current_position = rigidbody2d.position;
        move_dir.x = -current_position.x + Player.instance.transform.position.x;
        move_dir.y = -current_position.y + Player.instance.transform.position.y;
    }

    public virtual void Drop(int[] drop_ids, float[] drop_prob) 
    {
        
    }

    protected void Die(){
        Destroy(gameObject, 1.5f);
    }

    public void ChangeSpriteColor(float r, float g, float b, float alpha){
        float new_r = r;
        float new_g = g;
        float new_b = b;
        float new_alpha = alpha;
        if (r < 0)          new_r = sprite_render.color.r;
        if (g < 0)          new_g = sprite_render.color.g;
        if (b < 0)          new_b = sprite_render.color.b;
        if (alpha < 0)      new_alpha = sprite_render.color.a;
        sprite_render.color = new Color(new_r, new_g, new_b, new_alpha);
    }


    public void DisplayText()
    {
        Debug.Log("display text.");
    }

    


    public Vector2 GetDirectionToMove(){
        float[] danger = new float[8];
        float[] interest = new float[8];



        // foreach ()
        for (int i = 0; i < 8; i++){
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }





        Vector2 output_direction = Vector2.zero;
        return output_direction;
    }

}