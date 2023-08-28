using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TianXing : BaseBoss
{
    //float total_prepare_time = 2f;
    float prepare_interval = 1f;
    float prepare_interval_count = 0f;
    float scaler = 1;

    public float change_state_time = 5f;
    float change_state_time_count = 0f;

    float keep_direction_time = 1f;
    float keep_dir_count = 0f;
    Vector2 keep_dir = Vector2.zero;

    // attack1
    public float run_time = 0.15f;
    public float rest_interval = 0.4f;
    public float run_speed_mul_factor = 40f;
    float run_time_count = 0f;
    bool if_run = false;
    

    // attack2
    public float circle_remain_time = 5f;
    float circle_remain_time_count = 0f;
    public GameObject hole;
    public GameObject stone;
    public float stone_speed = 4;


    //attack3
    public GameObject crush;

    public float gen_crush_time_interval = 0.08f;
    float gen_crush_time_count = 0f;



    //attack4
    public int max_attacked_num = 10;
    public float grab_speed = 0.5f;

    //GameObject longjuan;

    //attack5
    public GameObject monster;
    int[] x_pos = new int[4] {3, 3, -3, -3};
    int[] y_pos = new int[4] {3, -3, -3, 3};
    bool if_gen_monster = false;



    public GameObject grab_sign;

    // Start is called before the first frame update
    void Start()
    {
        Init(1*(Player.instance.now_timeline+1));
        rigidbody2d.mass = 10;
        now_health = max_health;
        //longjuan = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if (!boss_initial){
            if (get_close){
                animator.SetBool("isMoving", false);
                // 准备动画，分两级变大，间隔一秒
                if (prepare_interval_count < prepare_interval){
                    scaler = 1.5f;
                    SetScale();
                    prepare_interval_count += Time.deltaTime;
                }
                else{
                    scaler = 2;
                    SetScale();
                    boss_state = Random.Range(2, 4);
                    boss_initial = true;
                    prepare_interval_count = 0f;
                    change_state_time_count = 0f;
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
                GenNewState(1, 6);
            }

            Debug.Log("boss state: " + boss_state.ToString());
            if (boss_state == 1){
                attack1(run_speed_mul_factor*move_speed);
                SetScale();
            }
            else if (boss_state == 2){
                animator.SetBool("AttackBomb", true);
                attack2(4, 4);
                SetScale();
            }
            else if (boss_state == 3){
                animator.SetBool("AttackBomb", true);
                attack3(Player.instance.transform.position);
                SetScale();
            }
            else if (boss_state == 4){
                attack4();
                //longjuan.SetActive(true);
                // animator.SetBool("AttackBomb", true);
                // transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (boss_state == 5){
                if (!if_gen_monster){
                    attack5(4);
                    if_gen_monster = true;
                }
                SetScale();
            }
            else{
                Debug.LogError("boss state error.");
            }
            


            if (now_health <= 0){
                boss_die = true;
            }
            change_state_time_count += Time.deltaTime;
        }
        else if (boss_die) {
            animator.SetBool("isMoving", false);
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

    public void attack1(float speed){
        if (!if_run && run_time_count == 0){
            if_run = true;
            run_time_count += Time.deltaTime;
        }
        else if (if_run && run_time_count < run_time){
            Move(speed);
            animator.SetBool("AttackRun", true);
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


    public void attack2(int attack_num_player, int attack_num_boss){
        animator.SetBool("isMoving", false);
        if (circle_remain_time_count == 0){
            Vector2 new_pos = Vector2.zero;
            for (int i = 0; i < attack_num_player; i++){
                new_pos = Player.instance.transform.position;
                new_pos.x += Random.Range(-3f, 3f);
                new_pos.y += Random.Range(-3f, 3f);
                GenCircle(new_pos);
                //GenStone(new_pos);
            }
            for (int i = 0; i < attack_num_boss; i++){
                new_pos = current_position;
                new_pos.x += Random.Range(-8f, 8f);
                new_pos.y += Random.Range(-8f, 8f);
                GenCircle(new_pos);
                //GenStone(new_pos);
            }
            circle_remain_time_count += Time.deltaTime;
        }
        else if (circle_remain_time_count >= circle_remain_time){
            circle_remain_time_count = 0f;
        }
        else
            circle_remain_time_count += Time.deltaTime;
    }

    void GenCircle(Vector2 center)
    {
        Debug.Log("gen circle");
        GameObject holeObject = Instantiate(hole, center, Quaternion.identity);
    }

    void GenStone(Vector2 center)
    {
        Vector2 bias = new Vector2(0, 5f);
        GameObject bulletObject = Instantiate(stone, bias + center, Quaternion.identity);
        StoneFall newBullet = bulletObject.GetComponent<StoneFall>();
        newBullet.SetScale(2, 2, 2);
        newBullet.SetSpeed(stone_speed);
        newBullet.SetExistTime(10f);
        newBullet.Launch(-bias.normalized, 300);   
    }

    public void attack3(Vector2 pos){
        animator.SetBool("isMoving", false);
        if (gen_crush_time_count <= gen_crush_time_interval){
            gen_crush_time_count += Time.deltaTime;
        }
        else{
            GenGroudCrack(pos);
            gen_crush_time_count = 0f;
        }
    }

    public void GenGroudCrack(Vector2 pos){
        GameObject crushObject = Instantiate(crush, pos, Quaternion.identity);
        Crush new_crush = crushObject.GetComponent<Crush>();
    }


    public void attack4(){
        animator.SetBool("isMoving", false);
        if (attacked_number < max_attacked_num){
            beatable = false;
            Grab(grab_speed); 
        }
        else
        {
            Grab(0f);
            beatable = true;
            boss_state = Random.Range(1, 5);
            change_state_time_count = 0f;
        }

    }
    void Grab(float speed){
        Vector2 grab_dir = Vector2.zero;
        grab_dir.x = current_position.x - Player.instance.transform.position.x;
        grab_dir.y = current_position.y - Player.instance.transform.position.y;
        grab_dir = grab_dir.normalized;
        Player.instance.ChangePos(grab_dir, speed);
    }

    public void attack5(int monster_num)
    {
        for (int i = 0; i < monster_num; i++){
            Vector2 new_pos = new Vector2(Player.instance.transform.position.x + x_pos[i], Player.instance.transform.position.y + y_pos[i]);
            GameObject monsterObject = Instantiate(monster, new_pos, Quaternion.identity);
            BaseBoss newmonster = monsterObject.GetComponent<BaseBoss>();
            //newmonster.player = player;
            newmonster.SetInitial(true);
            newmonster.move_speed = 1*move_speed;
            newmonster.base_health_per_level = 10;
        }
    }


    void OnCollisionStay2D(Collision2D other) {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null){
            a.ChangeHealth(-attack_value);
            if (boss_state == 1){
                GenNewState(2, 6);
            }
        }
    }

    public void GenNewState(int min, int max){
        animator.SetBool("AttackBomb", false);
        animator.SetBool("AttackRun", false);
        //longjuan.SetActive(false);
        boss_state = Random.Range(min, max);
        if (boss_state == 5){
            if_gen_monster = false;
        }
        if(boss_state == 4){
            grab_sign.SetActive(true);
        }
        else{
            grab_sign.SetActive(false);
        }
        Grab(0f);
        change_state_time_count = 0;
    }
}
