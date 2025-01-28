using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class InventoryUIVisual : MonoBehaviour
{

    [SerializeField] private List<GameObject> visualGameObjectList;

    void Start()
    {
        EventManager.Instance.OnInteractInventory += EventManager_OnInteractInventory; 
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

}
