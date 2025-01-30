using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ToolbarUI : MonoBehaviour
{

    [SerializeField] private GameObject hotbarItemPrefab;
    void Start()
    {
        EventManager.Instance.OnAddItemToHotbar += EventManager_OnAddItemToHotbar;
    }

    private void EventManager_OnAddItemToHotbar(object sender, EventManager.OnAddItemToHotbarEventArgs e){
        hotbarItemPrefab.GetComponent<Image>().sprite = e.inventoryObject.GetInventoryObjectSprite();
        hotbarItemPrefab.GetComponent<InventoryObjectUI>().SetInventoryObject(e.inventoryObject);
        GameObject go = Instantiate(hotbarItemPrefab, transform.GetChild(e.i));
    }


}
