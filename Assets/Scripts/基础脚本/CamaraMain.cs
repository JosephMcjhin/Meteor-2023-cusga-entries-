using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraMain : MonoBehaviour
{
    //�����Ŀ��
    public Transform target;

    //��������
    //private Vector3 dir;
    private void Start()
    {
        //���������ָ����ҵķ���ƫ����
        //dir = target.position - transform.position;
    }
    private void Update()
    {
        //ʱʱ�̼̿���������ĸ���λ��
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
