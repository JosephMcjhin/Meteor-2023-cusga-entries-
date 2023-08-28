using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Transform pre_parent;
    int index1;
    int index2;
    int from1;
    int from2;

    public GameObject Desc;

    public void OnBeginDrag(PointerEventData eventData){
        InventoryManager.instance.isok = false;
        transform.position = eventData.position;
        pre_parent = transform.parent;
        index1 = pre_parent.GetSiblingIndex();
        if(transform.parent.CompareTag("Merge") && index1 == 2 && GetComponent<Slot>().SlotItem.ItemID != "0000"){
            InventoryManager.DelPre();
        }
        if(transform.parent.CompareTag("Item")){
            from1 = 1;
        }
        else if(transform.parent.CompareTag("Weapon")){
            from1 = 2;
        }
        else if(transform.parent.CompareTag("Merge")){
            from1 = 3;
        }
        transform.SetParent(transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
        transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData){
        InventoryManager.instance.isok = true;
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
        else if(eventData.pointerCurrentRaycast.gameObject.transform.parent.CompareTag("Merge")){
            from2 = 3;
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

    void OnEnable(){
        Desc.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData){
        if(InventoryManager.instance.isok){
            Desc.transform.SetParent(transform.parent.parent.parent);
            Desc.SetActive(true);
        }         
    }

    public void OnPointerExit(PointerEventData eventData){
        Desc.transform.SetParent(transform);
        Desc.SetActive(false);
    }
}
