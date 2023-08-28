using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    // Start is called before the first frame update
    protected Rigidbody2D rigidbody2d;
    protected Vector2 dir;
    protected Vector2 current_position;
    protected Animator animator;
    public float exist_time;
    protected float now_time;

    public float speed = 1;
    protected float x_scale;

    public int damage;

    // int bullet_type = 0;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        now_time = exist_time;
        x_scale = transform.localScale.x;
    }

    public void Launch(Vector2 direction, float force)
    {
        float rotationAngle = 0f;
        if (direction.y >=0 && direction.x >=0)
            rotationAngle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        if (direction.y < 0 && direction.x >=0)
            rotationAngle =  Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        if (direction.y >=0 && direction.x < 0)
            rotationAngle = 180 + Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        if (direction.y < 0 && direction.x < 0)
            rotationAngle = 180 + Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        transform.right = direction;
        rigidbody2d.AddForce(direction * force);
        dir = direction;
    }

    protected Vector2 Judge2Direction()
    {
        Vector2 direction = new Vector2(0, 0);
        if (dir.x >= 0)         direction.Set(1, 0);
        else                    direction.Set(-1, 0);
        return direction;
    }

    public void SetScale()
    {
        Vector2 direction = Judge2Direction();
        float scale = direction.x + direction.y;
        //transform.localScale = new Vector3(scale * x_scale, transform.localScale.y, transform.localScale.z);
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
    }

    public void SetSpeed(float new_speed)
    {
        speed = new_speed;
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
        SetScale();
    }
}
