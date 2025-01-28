using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class InventoryObjectUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image image;
    private Transform parentAfterDrag;

    private void Awake(){
        image = transform.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData){
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
