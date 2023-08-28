using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSoldier : MonoBehaviour
{
    public float monsterMoveSpeed;
    public float HP;
    public float maxHP;
    public float followDistance;
    public float stepTimeInterval;
    public Transform target;

    public Collider2D monsterColl;
    public Animator animator;

    private float timeVal;
    private float stepTimeIntervalRecord;

    private float timer;
    private bool attack1 = false;
    private bool attack2 = false;
    private bool attack3 = false;

    public float awake_time = 0;
    float awake_time_count = 0;

    public int boss_id = 0;

    GameObject[] body;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        stepTimeIntervalRecord = stepTimeInterval;

        monsterColl = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MonsterMove();
        awake_time_count += Time.deltaTime;
        if (awake_time_count >= awake_time){
            body = GameObject.FindGameObjectsWithTag("Immortal");
            for(int i = 0; i < body.Length; i++){
                BaseBoss original_boss = body[i].GetComponent<BaseBoss>();
                if(original_boss.GetBossId() == boss_id){
                    body[i].SetActive(true);
                    break;
                }
            }
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        if (attack1 == false && attack2 == false && attack3 == false)
        {
            attack1 = true;
        }
        animator.SetBool("Attack1", attack1);
        animator.SetBool("Attack2", attack2);
        animator.SetBool("Attack3", attack3);

        if (attack1) 
        {
            attack1 = false;
            attack2 = true;
            attack3 = false;
        }
        else if (attack2)
        {
            attack1 = false;
            attack2 = false;
            attack3 = true;
        }
        else if (attack3) 
        {
            attack1 = false;
            attack2 = false;
            attack3 = false;
        }
    }

    public void SetBossId(int bossid){
        boss_id = bossid;
    }

    void MonsterMove()
    {
        int direction = Random.Range(0, 4);
        float xDelta = transform.position.x - target.position.x;
        float yDelta = transform.position.y - target.position.y;
        bool follow = (Mathf.Sqrt(Mathf.Pow(xDelta, 2) + Mathf.Pow(yDelta, 2)) <= followDistance); 

        if (follow)
        {
            if (0 >= xDelta + Mathf.Abs(yDelta))        direction = 2;
            else if (0 > -xDelta + Mathf.Abs(yDelta))   direction = 3;
            else if (0 >= Mathf.Abs(xDelta) + yDelta)   direction = 1;
            else                                        direction = 0;
            stepTimeInterval = stepTimeIntervalRecord / 3f;
            FollowPlayer();
        }
        // Debug.Log(direction);
        // else
        // {
        if (direction == 0)
            transform.Translate(Vector3.down * monsterMoveSpeed * Time.fixedDeltaTime, Space.Self);
        else if (direction == 1)
            transform.Translate(Vector3.up * monsterMoveSpeed * Time.fixedDeltaTime, Space.Self);
        else if (direction == 2)
        {
            transform.Translate(Vector3.right * monsterMoveSpeed * Time.fixedDeltaTime, Space.Self);
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.Translate(Vector3.left * monsterMoveSpeed * Time.fixedDeltaTime, Space.Self);
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        stepTimeInterval = stepTimeIntervalRecord;
        // }
        animator.SetInteger("AnimState", 1);
        animator.SetBool("Grounded", true);
    }

    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, monsterMoveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null)
        {
            a.ChangeHealth(-1);
            animator.SetInteger("AnimState", 0);
            if (timeVal >= stepTimeInterval)
            {
                Attack();
                timeVal = 0;
            }
            else
            {
                timeVal += Time.fixedDeltaTime;
            }
        }
        //Debug.Log("Projectile Collision with " + other.gameObject);
        //Destroy(gameObject);
    }

    public void Change_health(int x)
    {
        HP += x;
        // if (HP <= 0)
        // {
        //     Destroy(gameObject);
        // }
        // Debug.Log(HP);
    }
}

