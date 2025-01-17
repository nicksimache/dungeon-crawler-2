using UnityEngine;
using System;

public class InventoryObject : InteractObject
{
	[SerializeField] private Sprite inventoryObjectSprite;

	private void Update(){
		if(interactProgress == interactProgressMax){

			if(EventManager.Instance == null){
				Debug.LogError("No instance of event manager");
				return;
			}
			if(Player.Instance.GetNumInventoryObjects() < 5){
				EventManager.Instance.PickUpItem(this);
				gameObject.SetActive(false);
			} else {
				MakeCanInteract(false);
				interactProgress = 0f;
			}
		}
	}

	public Sprite GetInventoryObjectSprite(){
		return inventoryObjectSprite;
	}
}
