using UnityEngine;
using System;

public class InventoryObject : InteractObject
{
	
	private void Update(){
		if(interactProgress == interactProgressMax){
			gameObject.SetActive(false);
			EventManager.Instance.PickUpItem(this);			
		}
	}
}
