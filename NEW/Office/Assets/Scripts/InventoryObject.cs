using UnityEngine;
using System;

public class InventoryObject : InteractObject
{
	
	private void Update(){
		if(interactProgress == interactProgressMax){

			if(EventManager.Instance == null){
				Debug.LogError("No instance of event manager");
				return;
			}
			if(Player.Instance.GetPlayerInventoryObjectList().Count < 5){
				EventManager.Instance.PickUpItem(this);
				gameObject.SetActive(false);
			}
		}
	}
}
