using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JianGong : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;

    GameObject player;
    Rigidbody2D rigidbody2d;

    int flag = 0;

    Animator animator;
    Vector2 dir;

    public float[] charge_now = new float[5];

    public int max_health;
    int now_health;

    public HpBar mm;

    public Image[] charge_bar = new Image[5];

    public GameObject damage_prefab;


    public SpriteRenderer sp;

    void Charge_set(){
        /*
        float temp1 = 0;
        int temp11 = 0;
        for(int i=0;i<5;i++){
            temp1 += charge_now[i];
            if(charge_now[i] >= 1){
                temp11 ++;
            }
        }
        full_num.text = temp11.ToString();
        float temp2 = 0f;
        if(temp1 == 0){
            for(int i=0;i<5;i++){
                charge_bar[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            }
            return;
        }
        */
        for(int i=0;i<5;i++){
            //temp2 += charge_now[i];
            charge_bar[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.5f * charge_now[i]);
        }
    }

    public Boss_Dialog bd;

    void Awake()
    {
        //player = player.transform.Find("Player").gameObject;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        now_health = max_health;
        sp = GetComponent<SpriteRenderer>();
        charge_now = new float[5]; 
        Charge_set();
    }

    public void Charge_en(float[] charge){
        for(int i=0;i<5;i++){
            charge_now[i] += charge[i];
            charge_now[i] = Mathf.Min(1f,charge_now[i]);
        }
        bd.Speak();
        Charge_set();
    }

    public void Charge_clear(){
        charge_now = new float[5];
        Charge_set();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Move X", dir.x);
        animator.SetFloat("Move Y", dir.y);
    }

    void FixedUpdate()
    {
        if(flag == 0)player = GameObject.Find("Player");
        if(player == null)
        {
            return;
        }
        flag = 1;
        Vector2 position = rigidbody2d.position;
        dir.x = -position.x + player.transform.position.x;
        dir.y = -position.y + player.transform.position.y;
        dir = dir.normalized;
        position.x = position.x - speed * dir.x * Time.deltaTime;
        position.y = position.y - speed * dir.y * Time.deltaTime;
        //Debug.Log(dir);
        rigidbody2d.MovePosition(position);
    }

    IEnumerator Hurt(){
        sp.material.SetFloat("_FlashAmount", 0.8f);
        yield return new WaitForSeconds(.2f);
        sp.material.SetFloat("_FlashAmount", 0);
    }

    public void Change_health(int x)
    {
        now_health += x;
        mm.SetValue((float)now_health/(float)max_health);
        float temp1 = Random.Range(-1f,1f);
        float temp2 = Random.Range(-1f,1f);
        GameObject temp = Instantiate(damage_prefab, new Vector2(temp1+transform.position.x,temp2+transform.position.y), Quaternion.identity);
        temp.GetComponent<Damage>().da.text = x.ToString();
        StartCoroutine(Hurt());
        if (now_health <= 0)
        {
            print(123123123);
            Player.instance.main_sign = "kuangkengyiceng_1";
            SceneManager.LoadScene("矿坑一层");
            Destroy(gameObject);
        }
        //Debug.Log(now_health);
    }

    public void Change_pos(Vector2 delta){
        //Debug.Log(delta);
        transform.position = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y, transform.position.z);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        //���ǻ������˵�����־���˽�ɵ��������Ķ���
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null)
        {
            a.ChangeHealth(-1);
        }
        //Debug.Log("Projectile Collision with " + other.gameObject);
        //Destroy(gameObject);
    }
}
