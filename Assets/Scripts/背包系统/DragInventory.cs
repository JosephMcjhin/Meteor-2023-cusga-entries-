using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData){
    }

    public void OnDrag(PointerEventData eventData){
        Vector3 temp = new Vector3(transform.position.x + eventData.delta.x, transform.position.y + eventData.delta.y, transform.position.z);
        transform.position = temp;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData){
        
    }
}
