using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRobot : BaseBoss
{

    public GameObject mybullet;
    public float bullet_speed = 5f;

    public string robotname;

    public float attack_interval = 1f;
    float attack_time_count = 0f;
    float x_scale = 0f;
    void Start()
    {
        Init(1*(Player.instance.now_timeline+1));
        rigidbody2d.mass = 10;
        now_health = max_health;
        x_scale = transform.localScale.x;
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

    public void SetState(int new_state)
    {
        boss_state = new_state;
    }



    void Attack() {
        if (attack_time_count <= attack_interval){
            attack_time_count += Time.deltaTime;
        }
        else{
            animator.SetBool("isAttack", true);
            GameObject bulletObject = Instantiate(mybullet, current_position + move_dir.normalized*0.5f, Quaternion.identity);
            Shell newBullet = bulletObject.GetComponent<Shell>();
            newBullet.SetSpeed(bullet_speed);
            newBullet.SetExistTime(3f);
            newBullet.Launch(move_dir, Random.Range(20, 40));
            attack_time_count = 0;
        }
    }


    void FixedUpdate() {
        now_weapon_invisible --;
        now_weapon_invisible = Mathf.Max(0, now_weapon_invisible);
        CalculateDirection();
        float current_distance = Mathf.Sqrt(Mathf.Pow(move_dir.x, 2) + Mathf.Pow(move_dir.y, 2));
        // Debug.Log(current_distance);
        bool get_close = (current_distance <= prepare_distance);

        if (!boss_initial){
            if (get_close){
                SetScale();
                boss_initial = true;
            }
        }
        else if (boss_initial && !boss_die){
            if (!is_hurt){
                if (!get_close) {
                    animator.SetBool("isAttack", false);
                    Move(move_speed);
                }
                else{
                    Attack();
                }
                SetScale();
            }
            else{
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttack", false);
                AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
                bool playing_idle = stateinfo.IsName(robotname + "Idle");
                if (playing_idle)
                    is_hurt = false;
            }

            // life control
            if (now_health <= 0){
                boss_die = true;
            }
        }
        else if (boss_die) {
            animator.SetBool("isMoving", false);
            animator.SetBool("isHurt", false);
            animator.SetBool("isAttack", false);
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
    /*
    public void Change_health(int x)
    {
        now_health += x;
    }
    */

    void OnCollisionStay2D(Collision2D other)
    {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null)
        {
            a.ChangeHealth(-attack_value);
        }
    }

    // void Launch(){
    //     GameObject temp = Instantiate(bullet, bullet_position.position, Quaternion.identity);
    //     float bias = Random.Range(-5f,5f);
    //     temp.GetComponent<Bullet>().init(damage, charge, Quaternion.AngleAxis(bias,Vector3.forward)*lookDirection);
    // }
}
