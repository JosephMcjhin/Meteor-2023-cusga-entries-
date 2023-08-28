using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Transform pre_parent;
    Vector3 pre_position;

    void Revert(){
        transform.SetParent(pre_parent);
        transform.position = pre_position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData){
        pre_parent = transform.parent;
        pre_position = transform.position;
        if(gameObject.GetComponent<NSlot>().SlotNum1 != 0)transform.position = eventData.position;
        transform.SetParent(transform.parent.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData){
        if(gameObject.GetComponent<NSlot>().SlotNum1 != 0)transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData){
        if(gameObject.GetComponent<NSlot>().SlotNum1 == 0){
            Revert();
            return;
        }
        GameObject temp = eventData.pointerCurrentRaycast.gameObject;
        if(temp == null){
            Revert();
            return;
        }
        NSlot temp1 = temp.GetComponent<NSlot>();
        if(temp1 == null){
            Revert();
            return;
        }
        NSlot temp2 = gameObject.GetComponent<NSlot>();
        if(temp1.SlotBelong != 0 && temp1.SlotBelong != temp2.SlotItem.ItemClass){
            Revert();
            return;
        }
        NInventoryManager.instance.SwapList(temp1.SlotBelong, temp1.SlotID, temp2.SlotBelong, temp2.SlotID);
        NInventoryManager.instance.Refresh(temp1);
        NInventoryManager.instance.Refresh(temp2);
        CombatManager.instance.Loadweapon();
        Revert();
        return;
    }

    public void OnPointerClick(PointerEventData eventData){
        NInventoryManager.instance.SetDesc(gameObject.GetComponent<NSlot>().SlotItem.ItemInfo);
    }
}
