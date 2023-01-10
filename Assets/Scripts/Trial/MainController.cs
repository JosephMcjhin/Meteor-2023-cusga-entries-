using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Start is called before the first frame update
    float horizontal;
    float vertical;
    Vector2 lookDirection = new Vector2(1, 0);
    Animator animator;

    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;

    public int maxh;
    int nowh;

    public float invisible_time;
    float nowt;

    int Geth()
    {
        return nowh;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        nowh = maxh;
        nowt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        nowt -= Time.deltaTime;
        nowt = Mathf.Max(nowt, 0);

        animator.SetFloat("Move X", lookDirection.x);
        animator.SetFloat("Move Y", lookDirection.y);
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void Changeh(int x)
    {
        if (x < 0)
        {
            if (nowt > 0)
            {
                return;
            }
            nowh -= x;
        }
        else
        {
            nowh += x;
        }
    }
}
