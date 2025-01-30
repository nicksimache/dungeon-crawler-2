using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InventoryUIVisual : MonoBehaviour
{

    [SerializeField] private List<GameObject> visualGameObjectList;

    [SerializeField] private GameObject hotbarItemPrefab;

    private string TOOLBAR = "InventoryToolbar";

    void Start()
    {
        EventManager.Instance.OnInteractInventory += EventManager_OnInteractInventory; 
        EventManager.Instance.OnAddItemToInventory += EventManager_OnAddItemToInventory;

    }

    public void EventManager_OnInteractInventory(object sender, EventManager.OnInteractInventoryEventArgs e){
        if(e.openInventory){
            foreach (GameObject go in visualGameObjectList){
                go.SetActive(true);
            }
        }
        else {
            foreach(GameObject go in visualGameObjectList){
                go.SetActive(false);
            }
        }
    }

    private void EventManager_OnAddItemToInventory(object sender, EventManager.OnAddItemToInventoryEventArgs e){
        hotbarItemPrefab.GetComponent<Image>().sprite = e.inventoryObject.GetInventoryObjectSprite();
        hotbarItemPrefab.GetComponent<InventoryObjectUI>().SetInventoryObject(e.inventoryObject);
        GameObject go = Instantiate(hotbarItemPrefab, transform.Find(TOOLBAR).GetChild(e.i));
    }

}
