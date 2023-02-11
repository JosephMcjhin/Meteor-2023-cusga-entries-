using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    // Start is called before the first frame update
    //以下为单例模式的写法，并且切换场景时不会销毁主角
    public static Tank instance;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            if(instance != this){
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    //向量旋转angle度
    Vector2 rotate(Vector2 now, float angle){
        Vector2 temp;
        angle = angle * Mathf.PI / 180;
        temp.x = now.x * Mathf.Cos(angle) + now.y * Mathf.Sin(angle);
        temp.y = -now.x * Mathf.Sin(angle) + now.y * Mathf.Cos(angle);
        return temp;
    }

    //计时器更新
    public float invisible_time;
    float invisible_now;    //无敌计时器
    public float enemy_gen_time;
    float enemy_gen_now;    //敌人生成计时器
    float c_time(float t){
        t -= Time.deltaTime;
        t = Mathf.Max(t, 0);
        return t;
    }
    void Change_time(){
        invisible_now = c_time(invisible_now);
        enemy_gen_now = c_time(enemy_gen_now);
    }

    //按E的提示
    public GameObject sign;
    //背包
    public GameObject Inventory;

    //血量
    public int max_health;
    int now_health;

    public GameObject mask_slot;
    mask mask_now;
    public int Get_health(){
        return now_health;
    }
    public void Change_health(int x){
        if (x < 0)
        {
            if (invisible_now > 0)
            {
                return;
            }
            else
            {
                now_health += x;
                invisible_now = invisible_time;
                mask_now.SetValue((float)now_health/max_health);
                if (now_health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            now_health += x;
            now_health = Mathf.Min(now_health, max_health);
            mask_now.SetValue((float)now_health/max_health);
        }

        Debug.Log(now_health);
    }


    float horizontal;
    float vertical;
    Vector2 lookDirection;
    Animator animator;
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;
    public GameObject Enemy_Prefab;

    public GameObject Weapon;

    public GameObject Weapon_slot;

    // Update is called once per frame
    //角色移动，动画，射击
    void Update(){
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        Vector2 move = new Vector2(horizontal, vertical);
        //transform.right = lookDirection;

        if(Input.GetKeyDown("i")){
            bool Inventory_isopen = Inventory.activeSelf;
            Inventory.SetActive(!Inventory_isopen);
        }
        Change_time();
        if (enemy_gen_now == 0)
        {
            Enemy_gen();
            enemy_gen_now = enemy_gen_time;
        }
        animator.SetFloat("Move X", lookDirection.x);
        //animator.SetFloat("Move Y", lookDirection.y);
        animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        //Debug.Log(transform.right);
    }
    void FixedUpdate(){
        Vector2 position = rigidbody2d.position;
        //Debug.Log(horizontal);
        //Debug.Log(Sharp(horizontal));
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
        //Debug.Log(position);
    }

    //获取屏幕边缘坐标，生成敌人。
    Vector2 Cord_gen(Vector2 a1,Vector2 a2,float c){
        Vector2 temp;
        if (c >= 0 && c < 1)
        {
            temp.x = a1.x;
            temp.y = a1.y + c * (a2.y - a1.y);
            return temp;
        }
        if (c >= 1 && c < 2)
        {
            temp.y = a2.y;
            temp.x = a1.x + (c-1) * (a2.x - a1.x);
            return temp;
        }
        if(c>=2 && c < 3)
        {
            temp.x = a2.x;
            temp.y = a2.y - (c - 2) * (a2.x - a1.x);
            return temp;
        }
        temp.y = a1.y;
        temp.x = a2.x - (c - 3) * (a2.x - a1.x);
        return temp;
    }

    void Enemy_gen(){
        Vector2 StartPos = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));//����
        Vector2 EndPos = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));//����
        //Debug.Log(StartPos);
        //Debug.Log(EndPos);
        float a = Random.Range(0f, 4f);
        Vector2 temp = Cord_gen(StartPos, EndPos, a);
        GameObject projectileObject = Instantiate(Enemy_Prefab, temp, Quaternion.identity);
    }

    public string main_sign;    //用于场景转换



    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Application.targetFrameRate = -1;
        invisible_now = 0;
        enemy_gen_now = enemy_gen_time;
        now_health = max_health;
        Inventory.SetActive(false);
        main_sign = "";
        mask_now = mask_slot.GetComponent<mask>();
        mask_now.SetValue(1f);
    }
}
