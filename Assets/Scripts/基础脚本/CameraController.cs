using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float minx,maxx,miny,maxy;
    void Start(){
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void LateUpdate(){
        transform.position = Vector3.Lerp(transform.position,new Vector3(target.position.x,target.position.y,transform.position.z),speed*Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,minx,maxx),Mathf.Clamp(transform.position.y,miny,maxy),transform.position.z);
        
    }
}
