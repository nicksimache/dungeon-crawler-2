using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ChestUIVisual : MonoBehaviour
{
    [SerializeField] private List<Image> chestUIElementsList;

    void Start()
    {
        EventManager.Instance.OnOpenChest += EventManager_OnOpenChest; 
    }

    public void EventManager_OnOpenChest(object sender, EventManager.OnOpenChestEventArgs e){
        if(e.openChest){
            foreach (Image image in chestUIElementsList){
                image.gameObject.SetActive(true);
            }
        }
        else {
            foreach(Image image in chestUIElementsList){
                image.gameObject.SetActive(false);
            }
        }
    }

}
