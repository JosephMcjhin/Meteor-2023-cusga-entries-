using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chun : BaseBoss
{
    //float total_prepare_time = 2f;
    float prepare_interval = 1f;
    float prepare_interval_count = 0f;
    float scaler = 1;
    float original_scaler = 1;

    float keep_direction_time = 1f;
    float keep_dir_count = 0f;
    Vector2 keep_dir = Vector2.zero;

    public float change_state_time = 0f;
    float original_change_state_time = 0f;
    float change_state_time_count = 0f;

    int[] gen_x_pos = new int[6] {8, 16, 16, 16, 8, 0};
    int[] gen_y_pos = new int[6] {0, 0, -4, -7, -7, -7};

    int[] mid_stage_x_pos = new int[7] {0, 5, 7, 5, -5, -7, -5};
    int[] mid_state_y_pos = new int[7] {5, 4, 0, -4, -4, 0, 4};
    List<int> boss_idx = new List<int>();


    public float bullet_speed = 5f;
    public float sword_speed = 10f;

    // attack3
    public float run_time = 0.5f;
    public float rest_interval = 1f;
    float run_time_count = 0f;
    bool if_run = false;
    

    // attack2
    public GameObject chunjianqi;
    GameObject shield;
    public float gen_bullet_interval = 1f;
    float gen_bullet_time_count = 0f;
    //int current_bullet_idx = 0;
    int rotate_type = 0;
    int rotate_idx_count = 0;
    int[] rotate_number = new int[6] {0, 7, 5, 3, 3, 7};

    int[] rotate_angle1 = new int[7] {0, 15, 30, 45, 315, 330, 345};
    int[] rotate_angle2 = new int[5] {0, 30, 60, 300, 330};
    int[] rotate_angle3 = new int[3] {0, 45, 315};
    int[] rotate_angle4 = new int[3] {0, 36, 324};
    int[] rotate_angle5 = new int[7] {0, 20, 40, 60, 300, 320, 340};

    //attack1
    public GameObject sword;
    public float gen_sword_interval = 2f;
    float gen_sword_time_count = 0f;
    int[] sword_rotate = new int[7] {0, 15, 30, 45, -15, -30, -45};

    //mid attack
    public GameObject superswprd;
    public float mid_attack_bullet_interval = 0.1f;
    float mid_attack_count = 0f;
    bool if_mid_stage = false;
    int mid_state_count = 0;
    //bool correct_hint = false;

    // idle
    public float idle_time = 3f;


    //float idle_count = 0f;


    GameObject shock;
    int last_state = 4;

    // Start is called before the first frame update
    void Start()
    {
        Init(1*(Player.instance.now_timeline+1));
        rigidbody2d.mass = 10;
        now_health = max_health;
        original_change_state_time = change_state_time;
        shield = transform.GetChild(0).gameObject;
        // shock = transform.GetChild(1).gameObject;
    }

    public void SetScale()
    {
        Vector2 direction = Judge2Direction();
        float scale = direction.x + direction.y;
        if(scale < 0){
            sprite_render.flipX = true;
        }
        else{
            sprite_render.flipX = false;
        }
        //transform.localScale = new Vector3(scale * scale_x, transform.localScale.y, transform.localScale.z);
    }

    public void SetScale(Vector2 dir)
    {
        Vector2 direction = Judge2Direction(dir);
        float scale = direction.x + direction.y;
        if(scale < 0){
            sprite_render.flipX = true;
        }
        else{
            sprite_render.flipX = false;
        }
        //transform.localScale = new Vector3(scale * scale_x, transform.localScale.y, transform.localScale.z);
    }

    void FixedUpdate() {
        now_weapon_invisible --;
        now_weapon_invisible = Mathf.Max(0, now_weapon_invisible);
        CalculateDirection();
        float current_distance = Mathf.Sqrt(Mathf.Pow(move_dir.x, 2) + Mathf.Pow(move_dir.y, 2));
        bool get_close = (current_distance <= prepare_distance);

        if (!boss_initial){
            if (get_close){
                //animator.SetBool("isMoving", false);
                // 准备动画，分两级变大，间隔一秒
                if (prepare_interval_count < prepare_interval){
                    scaler = 1.5f;
                    original_scaler = scaler;
                    SetScale();
                    prepare_interval_count += Time.deltaTime;
                }
                else{
                    scaler = 3;
                    original_scaler = scaler;
                    SetScale();
                    GenNewState(1, last_state);
                    boss_initial = true;
                    prepare_interval_count = 0f;
                    gen_bullet_interval /= 2;
                    gen_sword_interval /= 2;
                    // shield.transform.localScale = new Vector3(scaler, scaler, scaler);
                }
            }
            else{
                if (keep_dir_count <= keep_direction_time){
                        current_position.x = current_position.x + (move_speed/2) * keep_dir.x * Time.deltaTime;
                        current_position.y = current_position.y + (move_speed/2) * keep_dir.y * Time.deltaTime;
                        rigidbody2d.MovePosition(current_position);
                        keep_dir_count += Time.deltaTime;
                    }
                else if(keep_dir_count <= 3*keep_direction_time){
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
                GenNewState(1, last_state);
                scaler = original_scaler;
            }
            if (!is_hurt){
                if (boss_state == 3){
                    attack3(5*move_speed);
                    SetScale();
                }
                else if (boss_state == 2){
                    shield.SetActive(true);
                    if (gen_bullet_time_count <= gen_bullet_interval) {
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
                        if (if_mid_stage)
                            attack4();
                    }
                    animator.SetBool("Defence", true);
                    Move(0.5f * move_speed);
                    SetScale();
                }
                else if (boss_state == 1){
                    if (gen_sword_time_count <= gen_sword_interval){
                        gen_sword_time_count += Time.deltaTime;
                    }
                    else{
                        attack1();
                        animator.SetBool("RemoteAttack", true);
                        gen_sword_time_count = 0;
                        if (if_mid_stage)
                            attack4();
                    }
                    SetScale();

                }
                else if (boss_state == 4){
                    if (mid_attack_count <= mid_attack_bullet_interval){
                        mid_attack_count += Time.deltaTime;
                    }
                    else{
                        attack4();
                        SetScale();
                        mid_attack_count = 0;
                    }

                    shock.SetActive(true);
                    Move(move_speed);
                    SetScale();
                }
                else{
                    Debug.LogError("boss state error.");
                }
            }
            else{
                animator.SetBool("RemoteAttack", false);
                animator.SetBool("ClosedAttack", false);
                animator.SetBool("Defence", false);
                AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
                bool playing_idle = stateinfo.IsName("ChunBossIdle");
                if (playing_idle)
                    is_hurt = false;
            }

            if (now_health <= 0){
                boss_die = true;
            }

            // 进入半血状态
            if (boss_id == 0 && now_health <=(max_health / 2)){
                if (!if_mid_stage && mid_state_count == 0){
                    // last_state++;
                    gen_bullet_interval /= 2.2f;
                    gen_sword_interval /= 2.2f;

                    if_mid_stage = true;
                    mid_state_count++;
                }
            }
            change_state_time_count += Time.deltaTime;
        }
        else if (boss_die) {
            animator.SetBool("isDie", true);
            Die();
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
            animator.SetBool("ClosedAttack", true);
            Move(speed);
            SetScale();
            run_time_count += Time.deltaTime;
        }
        else if (run_time_count >= run_time){
            animator.SetBool("ClosedAttack", false);
            if_run = false;
            if (run_time_count >= run_time + rest_interval){
                run_time_count = 0;
            }
            else{
                run_time_count += Time.deltaTime;
            }
        }
    }


    public void attack2(float rotate_angle){
        Vector2 current_dir = Vector2.zero;
        current_dir.x = -current_position.x + Player.instance.transform.position.x;
        current_dir.y = -current_position.y + Player.instance.transform.position.y;
        Vector2 new_angle = rotate(current_dir, rotate_angle);
        GameObject bulletObject = Instantiate(chunjianqi, current_position + new_angle.normalized*0.5f, Quaternion.identity);
        Shell newBullet = bulletObject.GetComponent<Shell>();
        newBullet.SetSpeed(bullet_speed);
        newBullet.SetExistTime(10f);
        newBullet.Launch(new_angle, Random.Range(20, 40));
    }



    public void attack1()
    {
        Vector2 current_dir = Vector2.zero;
        current_dir.x = -current_position.x + Player.instance.transform.position.x;
        current_dir.y = -current_position.y + Player.instance.transform.position.y;
        for (int i = 0; i < 7; i++) {
            Vector2 new_angle = rotate(current_dir.normalized, sword_rotate[i]);
            GameObject swordObject = Instantiate(sword, current_position+new_angle.normalized, Quaternion.identity);
            Sword newsword = swordObject.GetComponent<Sword>();
            newsword.SetSpeed(sword_speed);
            newsword.SetExistTime(5f);
            newsword.Launch(new_angle, 400);
        }
    }


    void OnCollisionEnter2D(Collision2D other) {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null){
            a.ChangeHealth(-attack_value);
            GenNewState(1, last_state);
        }
    }


    public void GenNewState(int begin, int end){
        boss_state = Random.Range(begin, end);
        animator.SetBool("RemoteAttack", false);
        animator.SetBool("ClosedAttack", false);
        animator.SetBool("Defence", false);
        shield.SetActive(false);
        // shock.SetActive(false);
        // animator.SetBool("isHurt", false);
        if (boss_state == 2){
            change_state_time *= 1.1f;
            rotate_type = Random.Range(1, 6);
            rotate_idx_count = 0;
        }
        else{
            change_state_time = original_change_state_time;
        }
        change_state_time_count = 0;
        // if_mid_stage = false;
    }


    public void attack4(){
        Vector2 current_dir = Vector2.zero;
        current_dir.x = -current_position.x + Player.instance.transform.position.x;
        current_dir.y = -current_position.y + Player.instance.transform.position.y;
        GameObject swordObject = Instantiate(superswprd, current_position+current_dir.normalized, Quaternion.identity);
        Sword newsword = swordObject.GetComponent<Sword>();
        newsword.SetSpeed(0.7f * sword_speed);
        newsword.SetExistTime(3f);
        newsword.Launch(current_dir, 50);
    }

    public void DestroyObj(){
        animator.SetBool("isDie", true);
        Destroy(gameObject, 0.8f);
    }
}