using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : BaseBoss
{

    //float total_prepare_time = 2f;
    float prepare_interval = 1f;
    float prepare_interval_count = 0f;
    float scaler = 1;
    float original_scaler = 1;

    float keep_direction_time = 1f;
    float keep_dir_count = 0f;
    Vector2 keep_dir = Vector2.zero;

    public GameObject prefabb;

    public float change_state_time = 0f;
    float original_change_state_time = 0f;
    float change_state_time_count = 0f;

    int[] gen_x_pos = new int[8] {6, 4, 4, -4, -4, -6, 0, 0};
    int[] gen_y_pos = new int[8] {0, 4, -4, 4, -4, 0, 6, -6};

    int[] mid_stage_x_pos = new int[8] {6, 4, 4, -4, -4, -6, 0, 0};
    int[] mid_state_y_pos = new int[8] {0, 4, -4, 4, -4, 0, 6, -6};
    List<int> boss_idx = new List<int>();

    public List<Magician> child_boss = new List<Magician>();

    public float bullet_speed = 5f;
    public float sword_speed = 10f;

    // attack3
    public float run_time = 0.5f;
    public float rest_interval = 1f;
    float run_time_count = 0f;
    bool if_run = false;
    

    // attack2
    public GameObject bullet;
    public float gen_bullet_interval = 0f;
    float gen_bullet_time_count = 0f;
    //int current_bullet_idx = 0;
    int rotate_type = 0;
    int rotate_idx_count = 0;
    int[] rotate_number = new int[6] {0, 24, 12, 8, 10, 18};

    int[] rotate_angle1 = new int[24] {0, 15, 30, 45, 60, 75, 90, 105, 120, 135, 150, 165, 180, 195, 210, 225, 240, 255, 270, 285, 300, 315, 330, 345};
    int[] rotate_angle2 = new int[12] {0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330};
    int[] rotate_angle3 = new int[8] {0, 45, 90, 135, 180, 225, 270, 315};
    int[] rotate_angle4 = new int[10] {0, 36, 72, 108, 144, 180, 216, 252, 288, 324};
    int[] rotate_angle5 = new int[18] {0, 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340};

    //attack1
    public GameObject sword;
    public float gen_sword_interval = 0f;
    float gen_sword_time_count = 0f;
    int[] sword_rotate = new int[3] {0, 15, -15};

    //mid attack
    public float mid_attack_bullet_interval = 0.5f;
    float mid_attack_count = 0f;
    bool if_mid_stage = false;
    int mid_state_count = 0;
    bool correct_hint = false;

    // idle
    public float idle_time = 3f;
    //float idle_count = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Init(1*(Player.instance.now_timeline+1));
        rigidbody2d.mass = 10;
        now_health = max_health;
        original_change_state_time = change_state_time;

        // weapons[weapon_id].SetActive(true);
        // weapon = weapons[weapon_id].GetComponent<BaseWeapon>();
        // weapon.SetBulletSpeed(100);
        // weapon.ChangeSpriteColor(-1f, -1f, -1f, 0f);
        // weapon.SetAttackInterval(0.4f);
    }

    public void SetScale()
    {
        Vector2 direction = Judge2Direction();
        float scale = direction.x + direction.y;

        transform.localScale = new Vector3(scale * scaler, scaler, scaler);
    }

    public void SetScale(Vector2 dir)
    {
        Vector2 direction = Judge2Direction(dir);
        float scale = direction.x + direction.y;

        transform.localScale = new Vector3(scale * scaler, scaler, scaler);
    }

    void FixedUpdate() {
        now_weapon_invisible --;
        now_weapon_invisible = Mathf.Max(0, now_weapon_invisible);
        CalculateDirection();
        float current_distance = Mathf.Sqrt(Mathf.Pow(move_dir.x, 2) + Mathf.Pow(move_dir.y, 2));
        bool get_close = (current_distance <= prepare_distance);
        if(boss_id != 0){
            now_health = max_health;
        }
        if (!boss_initial){
            if (get_close){
                animator.SetBool("isMoving", false);
                // 准备动画，分两级变大，间隔一秒
                if (prepare_interval_count < prepare_interval){
                    scaler = 1f;
                    original_scaler = scaler;
                    SetScale();
                    prepare_interval_count += Time.deltaTime;
                }
                else{
                    scaler = 1f;
                    original_scaler = scaler;
                    SetScale();
                    GenNewState(1, 4);
                    boss_initial = true;
                    prepare_interval_count = 0f;
                    InitializeBoss();
                    gen_bullet_interval /= 2;
                    gen_sword_interval /= 2;
                    ShuffleBoss(0, 8);
                }
            }
            else{
                if (keep_dir_count <= keep_direction_time){
                        current_position.x = current_position.x + (move_speed/2) * keep_dir.x * Time.deltaTime;
                        current_position.y = current_position.y + (move_speed/2) * keep_dir.y * Time.deltaTime;
                        animator.SetBool("isMoving", true);
                        rigidbody2d.MovePosition(current_position);
                        keep_dir_count += Time.deltaTime;
                    }
                else if(keep_dir_count <= 3*keep_direction_time){
                    animator.SetBool("isMoving", false);
                    keep_dir_count += Time.deltaTime;
                }
                else{
                    int temp_x = Random.Range(-3, 3);
                    if (temp_x * keep_dir.x < 0)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    keep_dir.x = temp_x;
                    keep_dir.y = Random.Range(-3, 3);
                    keep_dir = keep_dir.normalized;
                    keep_dir_count = 0;
                }
                SetScale(keep_dir);
            }
        }
        else if (boss_initial && !boss_die){
            if (change_state_time_count >= change_state_time){
                GenNewState(1, 4);
                scaler = original_scaler;
            }

            // Debug.Log("boss state: " + boss_state.ToString());

            if (boss_state == 3){
                attack3(2*move_speed);
                SetScale();
            }
            else if (boss_state == 2){
                if (gen_bullet_time_count <= gen_bullet_interval){
                    gen_bullet_time_count += Time.deltaTime;
                }
                else{
                    if (rotate_idx_count < rotate_number[rotate_type])
                    {
                        switch(rotate_type){
                            case 1:
                                attack2(rotate_angle1[rotate_idx_count]);
                                break;
                            case 2:
                                attack2(rotate_angle2[rotate_idx_count]);
                                break;
                            case 3:
                                attack2(rotate_angle3[rotate_idx_count]);
                                break;
                            case 4:
                                attack2(rotate_angle4[rotate_idx_count]);
                                break;
                            case 5:
                                attack2(rotate_angle5[rotate_idx_count]);
                                break;
                        }
                        rotate_idx_count += 1;
                    }
                    else{
                        rotate_type = Random.Range(1, 6);
                        rotate_idx_count = 0;
                    }
                    gen_bullet_time_count = 0;
                    animator.SetTrigger("Attack");
                }
                Move(0.5f * move_speed);
                SetScale();
            }
            else if (boss_state == 1){
                if (gen_sword_time_count <= gen_sword_interval){
                    gen_sword_time_count += Time.deltaTime;
                }
                else{
                    animator.SetTrigger("Attack");
                    attack1();
                    SetScale();
                    gen_sword_time_count = 0;
                }

            }
            else if (boss_state == 4){
                if (mid_attack_count <= mid_attack_bullet_interval){
                    mid_attack_count += Time.deltaTime;
                }
                else{
                    animator.SetTrigger("Attack");
                    MidAttack();
                    SetScale();
                    mid_attack_count = 0;
                }
            }
            else if (boss_state == 5){
                animator.SetBool("isMoving", false);
            }
            else{
                Debug.LogError("boss state error.");
            }
            


            if (now_health <= 0){
                boss_die = true;
            }

            // 进入半血状态
            if (boss_id == 0 && now_health <=(max_health / 2)){
                if (!if_mid_stage && mid_state_count == 0){
                    gen_bullet_interval /= 2.2f;
                    gen_sword_interval /= 2.2f;
                    MidStageInit();
                    for (int i = 0; i < child_boss.Count; i++){
                        child_boss[i].MidStageInit();
                    }
                    if_mid_stage = true;
                    mid_state_count++;
                }
            }

            // 进入1/4血状态
            if (boss_id == 0 && now_health <=(max_health / 4)){
                if (!if_mid_stage && mid_state_count == 1){
                    if (!correct_hint){
                        MidStageInit();
                        for (int i = 0; i < child_boss.Count; i++){
                            child_boss[i].MidStageInit();
                        }

                        if_mid_stage = true;
                        mid_state_count++;
                    }
                    else{
                        if(mid_state_count == 2){
                            gen_bullet_interval /= 2.5f;
                            gen_sword_interval /= 2.5f;
                            mid_state_count++;
                        }
                        
                    }
                }
            }
            change_state_time_count += Time.deltaTime;
        }
        else if (boss_die) {
            animator.SetBool("isMoving", false);
            animator.SetBool("isDie", true);
            Die();
            if (boss_id == 0 && !correct_hint){
                for (int i = 0; i < child_boss.Count; i++){
                    child_boss[i].DestroyObj();
                }
            }
        }
        else{
            ClearDirection();
            Move(move_speed);
            SetScale();
        }

        if (!beatable){
            unbeatable_time_count += Time.deltaTime;
            if (unbeatable_time_count >= unbeatable_time_interval){
                beatable = true;
                unbeatable_time_count = 0;
                animator.SetBool("isHurt", false);
            }
        }
        
    }

    public void attack3(float speed){
        if (!if_run && run_time_count == 0){
            if_run = true;
            run_time_count += Time.deltaTime;
        }
        else if (if_run && run_time_count < run_time){
            Move(speed);
            SetScale();
            run_time_count += Time.deltaTime;
        }
        else if (run_time_count >= run_time){
            if_run = false;
            animator.SetBool("isMoving", false);
            if (run_time_count >= run_time + rest_interval){
                run_time_count = 0;
            }
            else{
                run_time_count += Time.deltaTime;
            }
        }
    }


    public void attack2(float rotate_angle){
        animator.SetBool("isMoving", false);
        Vector2 current_dir = Vector2.zero;
        current_dir.x = -current_position.x + Player.instance.transform.position.x;
        current_dir.y = -current_position.y + Player.instance.transform.position.y;
        Vector2 new_angle = rotate(current_dir, rotate_angle);
        GameObject bulletObject = Instantiate(bullet, current_position + new_angle.normalized*0.5f, Quaternion.identity);
        Shell newBullet = bulletObject.GetComponent<Shell>();
        newBullet.SetSpeed(bullet_speed);
        newBullet.SetExistTime(10f);
        newBullet.Launch(new_angle, Random.Range(20, 40));
    }


    public void attack1()
    {
        animator.SetBool("isMoving", false);
        Vector2 current_dir = Vector2.zero;
        current_dir.x = -current_position.x + Player.instance.transform.position.x;
        current_dir.y = -current_position.y + Player.instance.transform.position.y;
        for (int i = 0; i < 3; i++){
            Vector2 new_angle = rotate(current_dir.normalized, sword_rotate[i]);
            GameObject swordObject = Instantiate(sword, current_position+new_angle.normalized, Quaternion.identity);
            Shell newsword = swordObject.GetComponent<Shell>();
            newsword.SetSpeed(sword_speed);
            newsword.SetExistTime(5f);
            newsword.Launch(new_angle, 400);
        }

    }


    void OnCollisionEnter2D(Collision2D other) {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null){
            a.ChangeHealth(-attack_value);
            if (boss_state == 3){
                GenNewState(1, 3);
            }
        }

        Bullet b = other.gameObject.GetComponent<Bullet>();
        if (b != null && b.GetBulletType() == 0){
            if (boss_id == 0 && boss_state == 4){
                GenNewState(5, 6);
                scaler = original_scaler;
                if (!correct_hint){
                    for (int i = 0; i < child_boss.Count; i++){
                        child_boss[i].DestroyObj();
                    }
                }
                correct_hint = true;
                gen_bullet_interval /= 1.2f;
                gen_sword_interval /= 1.2f;
                sword_speed *= 1.5f;
                bullet_speed *= 1.8f;
            }
        }
    }

    void InitializeBoss() {
        for (int boss_num = 0; boss_num < 7; boss_num++){
            Vector3 temp = new Vector3(transform.position.x + gen_x_pos[boss_num], transform.position.y + gen_y_pos[boss_num], transform.position.z);
            GameObject newObject = Instantiate(prefabb, temp, Quaternion.identity);
            BaseBoss newBaseBoss = newObject.GetComponent<BaseBoss>();
            //newBaseBoss.ChangeMaxHealth(10000);
            Magician newBoss = newObject.GetComponent<Magician>();
            newBoss.SetInitial(true);
            newBoss.GenNewState(1, 4);
            newBoss.SetBossId(boss_num + 1);
            newBoss.SetBossPos(boss_idx);
            newBoss.transform.GetChild(0).gameObject.SetActive(false);
            child_boss.Add(newBoss);
            child_boss_list.Add(newObject);
        }
        transform.position = new Vector3(transform.position.x + gen_x_pos[7], transform.position.y + gen_y_pos[7], transform.position.z);
    }


    public void GenNewState(int begin, int end){
        boss_state = Random.Range(begin, end);
        if (boss_state == 2){
            change_state_time *= 1f;
            rotate_type = Random.Range(1, 6);
            rotate_idx_count = 0;
        }
        else if (boss_state == 5){
            change_state_time = 3;
        }
        else{
            change_state_time = original_change_state_time;
        }
        change_state_time_count = 0;
        if_mid_stage = false;
    }

    public void ShuffleBoss(int min, int max){
        int random = Random.Range(min, max);
        for (int i = 0; i < 8; i++){
            while (boss_idx.Contains(random)){
                random = Random.Range(min, max);
            }
            boss_idx.Add(random);
        }
    }

    public void SetBossPos(List<int> boss_pos){
        boss_idx = boss_pos;
    }

    public void MidAttack(){
        animator.SetBool("isMoving", false);
        Vector2 current_dir = Vector2.zero;
        current_dir.x = -current_position.x + Player.instance.transform.position.x;
        current_dir.y = -current_position.y + Player.instance.transform.position.y;
        GameObject swordObject = Instantiate(sword, current_position+current_dir.normalized, Quaternion.identity);
        Sword newsword = swordObject.GetComponent<Sword>();
        newsword.SetSpeed(0.7f * sword_speed);
        newsword.SetExistTime(3f);
        newsword.Launch(current_dir, 50);
    }

    public void MidStageInit(){
        boss_state = 4;
        change_state_time = 2f;
        change_state_time_count = 0;
        scaler = 1;
        SetScale();
        Vector2 new_pos = new Vector2(Player.instance.transform.position.x + mid_stage_x_pos[boss_idx[boss_id]], Player.instance.transform.position.y + mid_state_y_pos[boss_idx[boss_id]]);
        rigidbody2d.MovePosition(new_pos);
        mid_attack_count = 0f;
    }

    public void DestroyObj(){
        animator.SetBool("isDie", true);
        for(int i=0;i<child_boss.Count;i++){
            if(child_boss[i] == null)continue;
            Destroy(child_boss[i].gameObject);
        }
        Destroy(gameObject, 0.8f);
    }
}
