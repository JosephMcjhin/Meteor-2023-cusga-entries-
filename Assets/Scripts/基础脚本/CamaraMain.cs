using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMain : MonoBehaviour
{
    //跟随的目标
    public Transform target;

    //方向向量
    //private Vector3 dir;
    private void Start()
    {
        //计算摄像机指向玩家的方向偏移量
        //dir = target.position - transform.position;
    }
    private void Update()
    {
        //时时刻刻计算摄像机的跟随位置
        if (target == null)
        {
            return;
        }

        Vector3 temp;
        temp.x = target.position.x;
        temp.y = target.position.y;
        temp.z = transform.position.z;
        transform.position = temp;
    }
}
