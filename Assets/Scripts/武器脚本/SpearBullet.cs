using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction)
    {
        transform.right = direction.normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //���ǻ������˵�����־���˽�ɵ��������Ķ���
        Enemy a = other.gameObject.GetComponent<Enemy>();
        if (a != null){
            a.Change_health(-1);
        }
    }

    // Update is called once per frame
}
