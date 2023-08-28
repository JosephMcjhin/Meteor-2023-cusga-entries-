using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : BaseBoss
{
    float keep_direction_time = 1f;
    float keep_dir_count = 0f;
    Vector2 keep_dir = Vector2.zero;


    void Start()
    {
        Init(1*(Player.instance.now_timeline+1));
        rigidbody2d.mass = 10;
        now_health = max_health;

        keep_dir.x = Random.Range(-3f, 3f);
        keep_dir.y = Random.Range(-3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScale()
    {
        Vector2 direction = Judge4Direction();
        if (direction.x == -1 && direction.y == 0)
        {
            animator.SetInteger("moveDir", 3);
            Debug.Log("left");
        }
        else if (direction.x == 1 && direction.y == 0){
            animator.SetInteger("moveDir", 1);
            Debug.Log("right");
        }

        else if (direction.x == 0 && direction.y == -1)
        {
            animator.SetInteger("moveDir", 0);
            Debug.Log("forward");
        }
        else if (direction.x == 0 && direction.y == 1){
            animator.SetInteger("moveDir", 2);
            Debug.Log("backward");
        }
    }

    public void SetScale(Vector2 dir)
    {
        Vector2 direction = Judge4Direction(dir);
        if (direction.x == -1 && direction.y == 0)
        {
            animator.SetInteger("moveDir", 3);
            Debug.Log(dir.x.ToString()+","+dir.y.ToString() +" left");
        }
        else if (direction.x == 1 && direction.y == 0){
            animator.SetInteger("moveDir", 1);
            Debug.Log(dir.x.ToString()+","+dir.y.ToString() +" right");
        }

        else if (direction.x == 0 && direction.y == -1)
        {
            animator.SetInteger("moveDir", 0);
            Debug.Log(dir.x.ToString()+","+dir.y.ToString() +" forward");
        }
        else if (direction.x == 0 && direction.y == 1){
            animator.SetInteger("moveDir", 2);
            Debug.Log(dir.x.ToString()+","+dir.y.ToString() +" backward");
        }
    }


    void FixedUpdate() {
        now_weapon_invisible --;
        now_weapon_invisible = Mathf.Max(0, now_weapon_invisible);
        CalculateDirection();
        float current_distance = Mathf.Sqrt(Mathf.Pow(move_dir.x, 2) + Mathf.Pow(move_dir.y, 2));
        bool get_close = (current_distance <= prepare_distance);

        if (!boss_initial){
            if (get_close){
                Move(move_speed);
                SetScale();
                boss_initial = true;
            }
            else{
                if (keep_dir_count <= keep_direction_time){
                    current_position.x = current_position.x + (move_speed/2) * keep_dir.normalized.x * Time.deltaTime;
                    current_position.y = current_position.y + (move_speed/2) * keep_dir.normalized.y * Time.deltaTime;
                    rigidbody2d.MovePosition(current_position);
                    keep_dir_count += Time.deltaTime;
                }
                else if(keep_dir_count <= 3*keep_direction_time){
                    keep_dir_count += Time.deltaTime;
                }
                else{
                    float temp_x = Random.Range(-3f, 3f);
                    keep_dir.x = temp_x;
                    keep_dir.y = Random.Range(-3f, 3f);
                    keep_dir_count = 0;
                }
                SetScale(keep_dir);
            }
        }
        else if (boss_initial && !boss_die){
            Move(move_speed);
            SetScale();
            if (now_health <= 0){
                boss_die = true;
            }
        }
        else if (boss_die) {
            animator.SetBool("isMoving", false);
            animator.SetInteger("moveDir", -1);
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

    void OnCollisionStay2D(Collision2D other)
    {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null)
        {
            a.ChangeHealth(-attack_value);
        }
    }
}
