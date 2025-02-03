using UnityEngine;
using System;

public class InventoryObject : InteractObject
{
	[SerializeField] private _BaseItem item;

	private void Update(){
		if(interactProgress == interactProgressMax){

			if(EventManager.Instance == null){
				Debug.LogError("No instance of event manager");
				return;
			}
			if(Player.LocalInstance.GetNumInventoryObjectsInHotbar() < 5){
				EventManager.Instance.PickUpItemIntoHotbar(this);
				gameObject.SetActive(false);
			} else if(Player.LocalInstance.GetNumInventoryObjectsOverall() < 20) {
				EventManager.Instance.PickUpItemIntoInventory(this);
				gameObject.SetActive(false);		
			} else {
				MakeCanInteract(false);
				interactProgress = 0f;
			}
			
		}
	}

	public Sprite GetInventoryObjectSprite(){
		return item.GetItemSprite();
	}

	public _BaseItem GetInventoryObjectSO(){
		return item;
	}
}
