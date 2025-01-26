using UnityEngine;
using System;
using System.Collections.Generic;

public class ChestObject : InteractObject
{

    private List<_BaseItem> chestItemList;

    private void Update(){
		if(interactProgress == interactProgressMax){
			if(EventManager.Instance == null){
				Debug.LogError("No instance of event manager");
				return;
			}
            else {
                EventManager.Instance.OpenChestUI(this, true);
            }
		}
		
	}

    public List<_BaseItem> GetChestItemList(){
        return chestItemList;
    }

    public void CloseChest(){
        EventManager.Instance.OpenChestUI(this, false);
    }
}
