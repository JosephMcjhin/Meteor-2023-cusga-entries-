using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    public float attack_time;
    public float defence_time;
    public float move_time;
    public float defence_attack_time;
    float now_time;
    float now_defence_attack_time;

    public float attack_per;
    public float defence_per;
    public float move_per;

    int now_state;

    Animator animator;
    AnimatorStateInfo animatorInfo;

    SpriteRenderer renderer1;

    public GameObject attack_zone;
    public GameObject bullet;
    public GameObject bullet_s;
    public Transform bullet_position;
    GameObject player; 

    Vector2 lookDirection;
    Vector3 flip;

    int time_1;

    public float minx,maxx,miny,maxy; 
    float deltax,deltay;
    float tox,toy;

    public GameObject shield;
    Shield dun;

    public float max_health;
    float now_health;
    
    bool flag;

    public GameObject mask_slot;
    mask mask_now;

    public GameObject text_pro;

    void Launch(float angle, GameObject bullet){
        GameObject projectileObject = Instantiate(bullet, bullet_position.position, Quaternion.identity);
        Bullet projectile = projectileObject.GetComponent<Bullet>();
        projectile.Launch(Quaternion.AngleAxis(angle,Vector3.forward)*lookDirection);
        //animator.SetTrigger("Launch");
    }

    void Change_time(ref float thistime){
        thistime -= Time.deltaTime;
        thistime = Mathf.Max(thistime,0);
    }

    void Change_dir(){
        Vector3 temp = (player.transform.position - transform.position).normalized;
        lookDirection.x = temp.x;
        lookDirection.y = temp.y;
        lookDirection = lookDirection.normalized;
        if(lookDirection.x < 0){
            transform.localScale = flip;
        }
        else{
            transform.localScale = new Vector3(-flip.x, flip.y, flip.z);
        }
    }

    public void Change_health(float x){
        now_health += x;
        now_health = Mathf.Max(0,now_health);
        mask_now.SetValue((float)now_health/max_health);
    }

    IEnumerator Fade(){
        for (float f = 1f; f >= 0; f -= 0.03f) {
            renderer1.color = new Color(1f,1f,1f,f);
            yield return new WaitForSeconds(.1f);
        }
        text_pro.SetActive(true);
        Destroy(gameObject);
    }

    IEnumerator Fade_in(){
        for (float f = 0f; f <= 1; f += 0.05f) {
            renderer1.color = new Color(1f,1f,1f,f);
            yield return new WaitForSeconds(.1f);
        }
    }

    void Choose_skill(){
        float temp = Random.Range(0,100);
        if(temp <= attack_per){
            animator.SetTrigger("攻击");
            now_state = 1;
            attack_zone.SetActive(true);
        }        
        else if(temp <= defence_per){
            animator.SetTrigger("防御");
            shield.SetActive(true); 
            now_state = 2;
        }
        else{
            animator.SetTrigger("位移");
            renderer1.color = new Color(1f,1f,1f,155f/255f);
            tox = Random.Range(minx,maxx);
            toy = Random.Range(miny,maxy);
            deltax = (tox - transform.position.x);
            deltay = (toy - transform.position.y);
            now_state = 3;
        }
    }

    public void ShootSword(){
        Launch(0,bullet);
        Launch(-30,bullet);
        Launch(-60,bullet);
        Launch(30,bullet);
        Launch(60,bullet);
    }

    void Attack(){
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime >= 1.0f){
            now_state = 0;
            now_time = attack_time;
            attack_zone.SetActive(false);
            animator.SetTrigger("恢复");
            return;
        }
    }

    void Defence(){
        Change_time(ref now_defence_attack_time);
        Change_dir();
        if(now_defence_attack_time == 0){
            Launch(0,bullet_s); 
            now_defence_attack_time = defence_attack_time;
        }
        if(dun.now_health == 0){
            now_state = 5;
            animator.SetTrigger("硬直");
            shield.SetActive(false);
            return;
        }
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime >= 1.0f){
            now_state = 0;
            now_time = defence_time;
            animator.SetTrigger("恢复");
            shield.SetActive(false);
            return;
        }
    }

    void Move(){
        Vector3 temp = new Vector3(transform.position.x + deltax*Time.deltaTime*3, transform.position.y + deltay*Time.deltaTime*3, transform.position.z);
        transform.position = temp;
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(animatorInfo.normalizedTime >= 1.0f){
            now_state = 0;
            now_time = move_time;
            temp = new Vector3(tox,toy,transform.position.z);
            transform.position = temp;
            renderer1.color = new Color(1f,1f,1f,1f);
            animator.SetTrigger("恢复");
            return;
        }
    }

    void Idle(){
        Change_time(ref now_time);
        Change_dir();
        if(now_time == 0){
            Choose_skill();
        }
    }

    void OnEnable(){
        now_time = 3;
        now_state = 0;
        now_health = max_health;
        animator = GetComponent<Animator>();
        renderer1 = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        flip = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        dun = shield.GetComponent<Shield>();
        shield.SetActive(false);
        StartCoroutine("Fade_in");
        mask_now = mask_slot.GetComponent<mask>();
    }

    void Update(){
        if(now_health == 0 && now_state != 4){
            animator.SetTrigger("死亡");
            renderer1.color = new Color(1f,1f,1f,1f);
            now_state = 4;
            return;
        }
        if(now_state == 0){
            Idle();
        }
        else if(now_state == 1){
            Attack();
        }
        else if(now_state == 2){
            Defence();
        }
        else if(now_state == 3){
            Move();
        }
        else if(now_state == 4){
            if(!flag){
                StartCoroutine("Fade");
                flag = true;
            }
        }
        else{
            animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
            if(animatorInfo.normalizedTime >= 1.0f){
                now_state = 0;
                now_time = defence_time;
            }
        }
    }
}
