using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InventoryToolbarUI : MonoBehaviour
{
    [SerializeField] private GameObject hotbarItemPrefab;
    void Start()
    {
        EventManager.Instance.OnAddItemToInventory += EventManager_OnAddItemToInventory;
    }

    private void EventManager_OnAddItemToInventory(object sender, EventManager.OnAddItemToInventoryEventArgs e){
        hotbarItemPrefab.GetComponent<Image>().sprite = e.inventoryObject.GetInventoryObjectSprite();
        hotbarItemPrefab.GetComponent<InventoryObjectUI>().SetInventoryObject(e.inventoryObject);
        GameObject go = Instantiate(hotbarItemPrefab, transform.GetChild(e.i));
    }
}
