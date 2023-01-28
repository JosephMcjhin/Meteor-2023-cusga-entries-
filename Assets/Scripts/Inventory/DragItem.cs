using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform pre_parent;
    int index1;
    int index2;
    int from1;
    int from2;
    public void OnBeginDrag(PointerEventData eventData){
        transform.position = eventData.position;
        pre_parent = transform.parent;
        index1 = pre_parent.GetSiblingIndex();
        if(transform.parent.CompareTag("Item")){
            from1 = 1;
        }
        else if(transform.parent.CompareTag("Weapon")){
            from1 = 2;
        }
        transform.SetParent(transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
        transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData){
        if(eventData.pointerCurrentRaycast.gameObject == null){
            transform.SetParent(pre_parent);
            transform.position = pre_parent.position;
        }
        else if(eventData.pointerCurrentRaycast.gameObject.transform.parent.CompareTag("Item")){
            from2 = 1;
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.position = pre_parent.position;
            index2 = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetSiblingIndex();
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(pre_parent);
            Debug.Log(index1);
            Debug.Log(index2);
            Debug.Log(from1);
            Debug.Log(from2);
            InventoryManager.SwapList(index1,from1,index2,from2);
        }
        else if(eventData.pointerCurrentRaycast.gameObject.transform.parent.CompareTag("Weapon")){
            from2 = 2;
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.position = pre_parent.position;
            index2 = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetSiblingIndex();
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(pre_parent);
            Debug.Log(index1);
            Debug.Log(index2);
            Debug.Log(from1);
            Debug.Log(from2);
            InventoryManager.SwapList(index1,from1,index2,from2);
        }
        else{
            transform.SetParent(pre_parent);
            transform.position = pre_parent.position;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
