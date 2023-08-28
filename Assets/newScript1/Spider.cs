using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : BaseBoss
{
    float keep_direction_time = 1f;
    float keep_dir_count = 0f;
    Vector2 keep_dir = Vector2.zero;
    float scale_x;


    void Start()
    {
        Init(1*(Player.instance.now_timeline+1));
        rigidbody2d.mass = 10;
        now_health = max_health;

        keep_dir.x = Random.Range(-3, 3);
        keep_dir.y = Random.Range(-3, 3);
        keep_dir = keep_dir.normalized;

        scale_x = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

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
                RandomMove(move_speed);
                // Move(move_speed);
                SetScale();
                boss_initial = true;
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
            // RandomMove(move_speed);
            Move(move_speed);
            SetScale();
            if (now_health <= 0){
                boss_die = true;
            }
        }
        else if (boss_die) {
            animator.SetBool("isMoving", false);
            Die();
        }
        else{
            ClearDirection();
            // RandomMove(move_speed);
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

    void OnCollisionStay2D(Collision2D other)
    {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null)
        {
            a.ChangeHealth(-attack_value);
        }
    }
}
