using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFall : MonoBehaviour
{
// Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    Vector2 dir;
    Vector2 current_position;
    public float exist_time;
    float now_time;

    public float speed = 1;

    public int damage;


    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
        dir = direction;
    }


    public void SetExistTime(float new_time)
    {
        exist_time = new_time;
        now_time = exist_time;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Player a = other.gameObject.GetComponent<Player>();
        if (a != null){
            a.ChangeHealth(-damage);
            Destroy(gameObject);
        }

        Hole b = other.gameObject.GetComponent<Hole>();
        if (b != null)
        {
            Debug.Log("hit hole");
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float new_speed)
    {
        speed = new_speed;
    }

    public void SetScale(float x, float y, float z){
        transform.localScale = new Vector3(x, y, z);
    }


    void FixedUpdate()
    {
        if (now_time < 0)
        {
            Destroy(gameObject);
        }
        dir = dir.normalized;
        current_position = rigidbody2d.position;
        current_position.x = current_position.x + speed * dir.x * Time.deltaTime;
        current_position.y = current_position.y + speed * dir.y * Time.deltaTime;
        rigidbody2d.MovePosition(current_position);
        now_time -= Time.deltaTime;
    }
}
