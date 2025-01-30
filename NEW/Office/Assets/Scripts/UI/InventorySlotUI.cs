using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData){
        if(transform.childCount == 0){
            InventoryObjectUI inventoryObjectUI = eventData.pointerDrag.GetComponent<InventoryObjectUI>();
            inventoryObjectUI.parentAfterDrag = transform;
        }
    }
}
