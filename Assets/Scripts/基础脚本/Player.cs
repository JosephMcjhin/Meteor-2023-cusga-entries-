using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    //以下为单例模式的写法，并且切换场景时不会销毁主角
    public static Player instance;
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

    //计时器更新
    public float invisible_time;
    float invisible_now;    //无敌计时器
    public float recover_time;
    float recover_now;
    void ChangeTime(){
        invisible_now = Mathf.Max(invisible_now - Time.deltaTime, 0);
        recover_now = Mathf.Max(recover_now - Time.deltaTime, 0);
    }

    //属性
    public int max_health;
    int now_health;
    public float speed;
    public int now_shield;
    public int recover;
    public float attack_speed;

    public Image[] charge_bar = new Image[5];
    public float[] charge_now = new float[5];

    public float speed_boost;
    public float attack_boost;
    public float damage_boost;     //元素伤害

    public GameObject shield_sign;
    public GameObject recover_sign;
    public GameObject damage_prefab;


    //按E的提示
    public GameObject sign;
    //血条
    public HpBar hp_bar;
    public void ChangeHealth(int x){
        if(x < 0 && invisible_now > 0)return;
        if(x < 0) {
            invisible_now = invisible_time;
            if(now_shield > 0){
                int temp11 = Mathf.Min(x, now_shield);
                now_shield -= temp11;
                x-= temp11;
            }
            CombatManager.instance.last_hurt = CombatManager.instance.total_time;
        }
        now_health += x;
        now_health = Mathf.Clamp(now_health, 0, max_health);
        float temp1 = Random.Range(-1f,1f);
        float temp2 = Random.Range(-1f,1f);
        GameObject temp = Instantiate(damage_prefab, new Vector2(temp1+transform.position.x,temp2+transform.position.y), Quaternion.identity);
        temp.GetComponent<Damage>().damage = x;
        if(now_health <= 0)Destroy(gameObject);
        hp_bar.SetValue((float)now_health/max_health);
    }
    void ChargeSet(){
        for(int i=0;i<5;i++){
            charge_bar[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140f * charge_now[i]);
        }
    }
    public void ChargeClear(){
        charge_now = new float[5];
        ChargeSet();
    }
    public void ChargeEn(float[] charge){
        for(int i=0;i<5;i++){
            charge_now[i] += charge[i];
            charge_now[i] = Mathf.Min(1f,charge_now[i]);
        }
        ChargeSet();
    }
    public void ChangePos(Vector2 delta, float xx){
        transform.position = new Vector3(transform.position.x + delta.x * xx, transform.position.y + delta.y * xx, transform.position.z);
    }
    Animator animator;
    Rigidbody2D rigidbody2d;

    public GameObject Weapon;

    public GameObject Weapon_slot;

    //移动相关变量
    Vector2 lookDirection;
    public Vector2 mousePos;
    Vector2 moveDir;
    Vector3 flip;
    SpriteRenderer sp;
    void ChangeDir(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDirection = (mousePos - new Vector2(transform.position.x,transform.position.y)).normalized;
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if(mousePos.x < transform.position.x)sp.flipX = true;
        else sp.flipX = false;
    }
    void Update(){
        ChangeDir();
        ChangeTime();
        if(recover_now <= 0){
            if(recover > 0){
                ChangeHealth(recover);
                recover_now = recover_time;
            }
        }
        animator.SetFloat("Speed", moveDir.magnitude);
        if(Input.GetKeyDown("f")){
            GetComponent<BoxCollider2D>().enabled = !GetComponent<BoxCollider2D>().enabled;
        }
    }
    void FixedUpdate(){
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * moveDir.x * Time.deltaTime * speed_boost;
        position.y = position.y + speed * moveDir.y * Time.deltaTime * speed_boost;
        rigidbody2d.MovePosition(position);
    }

    public string main_sign;    //用于场景转换

    //状态变量
    public int treasure_found;  //发现宝藏的个数

    public int now_timeline;    //当前所处的时间线

    public PlayerState player_state;
    public Camera now_camera;
    public GameObject teleport_sign;

    void Start(){
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        invisible_now = 0;
        now_health = max_health;
        main_sign = "";
        treasure_found = 0;
        speed_boost = 1;
        attack_boost = 1;
        charge_now = new float[5]; 
        ChargeSet();
    }
}
