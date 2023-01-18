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
    public float bullet_time;
    float bullet_now;   //子弹计时器
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
        bullet_now = c_time(bullet_now);
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
                mask.instance.SetValue((float)now_health/max_health);
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
            mask.instance.SetValue((float)now_health/max_health);
        }

        Debug.Log(now_health);
    }


    float horizontal;
    float vertical;
    float lookx;
    float looky;
    Vector2 lookDirection = new Vector2(0, 1);
    Animator animator;
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;
    public GameObject projectilePrefab;
    public GameObject Enemy_Prefab;

    // Update is called once per frame
    //角色移动，动画，射击
    void Update(){
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Pos.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        lookx = mousePos.x - transform.position.x;
        looky = mousePos.y - transform.position.y;
        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(lookx, 0.0f) || !Mathf.Approximately(looky, 0.0f))
        {
            lookDirection.Set(lookx, looky);
            lookDirection.Normalize();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (bullet_now == 0)
            {
                Launch();
                bullet_now = bullet_time;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (bullet_now == 0)
            {
                Launch3();
                bullet_now = bullet_time;
            }
        }

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
        animator.SetFloat("Move Y", lookDirection.y);
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        Debug.Log(Input.mousePosition);
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

    //射击
    void Launch(){
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + lookDirection * 0.5f, Quaternion.identity);

        Bullet projectile = projectileObject.GetComponent<Bullet>();
        projectile.Launch(lookDirection, 300);

        //animator.SetTrigger("Launch");
    }
    void Launch3(){
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + lookDirection * 0.5f, Quaternion.Euler(0, 0, 0));
        Bullet projectile = projectileObject.GetComponent<Bullet>();
        projectile.Launch(lookDirection, 300);
        projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + rotate(lookDirection, 35) * 0.5f, Quaternion.Euler(0, 0, 0));
        projectile = projectileObject.GetComponent<Bullet>();
        projectile.Launch(rotate(lookDirection, 35), 300);
        projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + rotate(lookDirection, -35) * 0.5f, Quaternion.Euler(0, 0, 0));
        projectile = projectileObject.GetComponent<Bullet>();
        projectile.Launch(rotate(lookDirection, -35), 300);
        //animator.SetTrigger("Launch");
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

    public string main_sign;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        //Application.targetFrameRate = -1;
        bullet_now = 0;
        invisible_now = 0;
        enemy_gen_now = 0;
        now_health = max_health;
        Inventory.SetActive(false);
        mask.instance.SetValue((float)1);
        main_sign = "";
    }
}
