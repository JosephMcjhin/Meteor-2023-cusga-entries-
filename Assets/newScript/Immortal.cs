using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Immortal : BaseBoss
{

    int[] x_pos = new int[6] {8, 16, 16, 16, 8, 0};
    int[] y_pos = new int[6] {0, 0, -4, -7, -7, -7};

    int die_count = 0;
    float x_transform = 0f;
    float y_transform = 0f;

    float keep_direction_time = 1f;
    float keep_dir_count = 0f;
    Vector2 keep_dir = Vector2.zero;

    public float attack_time_interval = 1f;
    float attack_time_count = 0f;

    public GameObject attack_jianqi;
    
    void Start()
    {
        Init(1*(Player.instance.now_timeline+1));
        rigidbody2d.mass = 10;
        now_health = max_health;

        x_transform = Mathf.Abs(transform.localScale.x);
        y_transform = Mathf.Abs(transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScale()
    {
        Vector2 direction = Judge2Direction();
        float scale = direction.x + direction.y;

        // 竹林贤者的徒弟
        if (boss_id != 0)
            transform.localScale = new Vector3(scale * x_transform / 1.3f, y_transform / 1.3f, transform.localScale.z);
        else
            transform.localScale = new Vector3(scale * x_transform, transform.localScale.y, transform.localScale.z);
    }

    public void SetScale(Vector2 dir)
    {
        Vector2 direction = Judge2Direction(dir);
        float scale = direction.x + direction.y;

        if (boss_id != 0)
            transform.localScale = new Vector3(scale * x_transform / 1.3f, y_transform / 1.3f, transform.localScale.z);
        else
            transform.localScale = new Vector3(scale * x_transform, transform.localScale.y, transform.localScale.z);
    }


    void InitializeBoss() {
        for (int boss_num = 0; boss_num < 6; boss_num++){
            Vector3 temp = new Vector3(transform.position.x + x_pos[boss_num], transform.position.y + y_pos[boss_num], transform.position.z);
            GameObject newObject = Instantiate(gameObject, temp, Quaternion.identity);
            Immortal newBoss = newObject.GetComponent<Immortal>();
            newBoss.SetInitial(true);
            newBoss.move_speed = move_speed * 1.5f;
            newBoss.SetBossId(boss_num + 1);
        }
    }


    void Attack() {
        if (attack_time_count > attack_time_interval){
            animator.SetTrigger("Attack");
            GameObject bulletObject = Instantiate(attack_jianqi, current_position, Quaternion.identity);
            Shell newBullet = bulletObject.GetComponent<Shell>();
            newBullet.SetSpeed(5f);
            newBullet.Launch(rotate(move_dir.normalized, 0), 300);
            attack_time_count = 0;
        }
        else{
            attack_time_count += Time.deltaTime;
        }
    }


    void FixedUpdate() {
        if (!hidden){
            CalculateDirection();
            float current_distance = Mathf.Sqrt(Mathf.Pow(move_dir.x, 2) + Mathf.Pow(move_dir.y, 2));
            bool get_close = (current_distance <= prepare_distance);

            if (!boss_initial){
                if (get_close){
                    Move(move_speed);
                    SetScale();
                    boss_initial = true;
                    InitializeBoss();
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
                if (!is_hurt){
                    if (current_distance + Random.Range(-1.5f, 1.5f) >= prepare_distance){
                        Move(move_speed);
                    }
                    else{
                        animator.SetBool("isMoving", false);
                    }
                    Attack();
                    SetScale();
                }
                else{
                    animator.SetBool("isMoving", false);
                    AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
                    bool playing_idle = stateinfo.IsName("ImmortalIdle");
                    if (playing_idle)
                        is_hurt = false;
                }

                if (now_health <= 0){
                    boss_die = true;
                }
            }
            else if (boss_die && (die_count == 0)) {
                animator.SetBool("isMoving", false);
                Change();
                die_count += 1;
            }
            else if (die_count == 1){
                animator.SetBool("isMoving", false);
                animator.SetBool("isDie", true);
                Die();
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

        else{
            hidden_time_count += Time.deltaTime;
            if (hidden_time_count >= hidden_time){
                hidden = false;
                hidden_time_count = 0;
                boss_die = false;
                now_health = max_health;
                sprite_render.color = new Color(sprite_render.color.r, sprite_render.color.g, sprite_render.color.b, 1f);
                animator.SetBool("isHurt", false);
            }
        }
        
    }

    public void Change_health(int x)
    {
        now_health += x;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null)
        {
            a.ChangeHealth(-attack_value);
        }
    }


    void GenSoldier() {
        int[] x_pos = new int[4] {0, 0, 3, -3};
        int[] y_pos = new int[4] {3, -3, 0, 0};

        for (int soldier_num = 0; soldier_num < 2; soldier_num++){
            Vector3 temp = new Vector3(transform.position.x + x_pos[soldier_num], transform.position.y + y_pos[soldier_num], transform.position.z);
            GameObject newObject = Instantiate(child_boss_list[0], temp, Quaternion.identity);
            BaseBoss newSoldier = newObject.GetComponent<BaseBoss>();
            //newSoldier.player = player;
            newSoldier.move_speed = move_speed / 2;
            newSoldier.SetInitial(true);
            newSoldier.SetBossId(boss_id);
        }
    }

    void Change(){
        GenSoldier();
        ChangeSpriteColor(-1, -1, -1, 0.2f);
        hidden = true;
    }
}
