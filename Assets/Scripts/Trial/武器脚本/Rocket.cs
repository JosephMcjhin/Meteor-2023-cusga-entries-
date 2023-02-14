 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    //Animator animator;
    public float exist_time;
    float now_time;

    public float speed;
    public float spin_speed;

    bool arrived;

    public Vector2 targetpos;

    public GameObject exp;

    void Awake()
    {
        //animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
    }

    public void Launch(Vector2 direction)
    {
        //Debug.Log(direction.x);
        //Debug.Log(direction.y);
        rigidbody2d.velocity = direction * speed;
        transform.right = direction.normalized;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //���ǻ������˵�����־���˽�ɵ��������Ķ���
        Enemy a = other.gameObject.GetComponent<Enemy>();
        Shield b = other.gameObject.GetComponent<Shield>();
        boss c = other.gameObject.GetComponent<boss>();
        if (a != null)
        {
            a.Change_health(-1);
        }
        if(b != null){
            b.Change_health(-1);
        }
        if(c != null){
            c.Change_health(-1);
        }
        //Debug.Log("Projectile Collision with " + other.gameObject);
        Instantiate(exp, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate(){
        dir = (targetpos - new Vector2(transform.position.x,transform.position.y)).normalized;
        if(!arrived){
            transform.right = Vector3.Slerp(transform.right,dir,spin_speed/Vector2.Distance(transform.position,targetpos));
            rigidbody2d.velocity = transform.right * speed;
        }
        if(Vector2.Distance(transform.position,targetpos)<1 && !arrived){
            arrived = true;
        }
    }

    void Update()
    {
        if (now_time < 0)
        {
            Instantiate(exp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //animator.SetFloat("Look X", dir.x);
        //animator.SetFloat("Look Y", dir.y);
        now_time -= Time.deltaTime;
    }
}
