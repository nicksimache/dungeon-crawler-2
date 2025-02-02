using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

   	private void Awake() {
		if(Instance == null){
			Instance = this;
		}
		else {
			Destroy(gameObject);
		}
    }

    public event EventHandler<OnPickUpItemEventArgs> OnPickUpItem;
    public class OnPickUpItemEventArgs : EventArgs {
		public InventoryObject inventoryObject;
		public bool itemGoesIntoHotbar;
    }

	public event EventHandler<OnInteractInventoryEventArgs> OnInteractInventory;
	public class OnInteractInventoryEventArgs : EventArgs {
		public bool openInventory;
	}

	public event EventHandler<OnAddItemToHotbarEventArgs> OnAddItemToHotbar;
	public class OnAddItemToHotbarEventArgs : EventArgs {
		public int i;
		public InventoryObject inventoryObject;
	}

	public event EventHandler<OnAddItemToInventoryEventArgs> OnAddItemToInventory;
	public class OnAddItemToInventoryEventArgs : EventArgs {
		public int i;
		public InventoryObject inventoryObject;
	}

	public event EventHandler<OnAccessTerminalEventArgs> OnAccessTerminal;
	public class OnAccessTerminalEventArgs : EventArgs {
		public bool openTerminal;
	}

	public event EventHandler<OnOpenChestEventArgs> OnOpenChest;
	public class OnOpenChestEventArgs : EventArgs {
		public bool openChest;
	}

    public void PickUpItemIntoHotbar(InventoryObject inventoryObject){
		OnPickUpItem?.Invoke(this, new OnPickUpItemEventArgs {
			inventoryObject = inventoryObject,
			itemGoesIntoHotbar = true
		});
    }

	public void PickUpItemIntoInventory(InventoryObject inventoryObject){
		OnPickUpItem?.Invoke(this, new OnPickUpItemEventArgs {
			inventoryObject = inventoryObject,
			itemGoesIntoHotbar = false
		});
    }

	public void AccessTerminal(bool openTerminal){
		OnAccessTerminal?.Invoke(this, new OnAccessTerminalEventArgs{
			openTerminal = openTerminal
		});
	}

	public void OpenChestUI(ChestObject chestObject, bool openChest){
		OnOpenChest?.Invoke(this, new OnOpenChestEventArgs{
			openChest = openChest
		});
	}

	public void OpenInventory(bool openInventory){
		/*
		if(!Player.Instance.IsPlayerUsingComputer()){
			OnInteractInventory?.Invoke(this, new OnInteractInventoryEventArgs{
				openInventory = openInventory
			});
		}
		*/
		
	}

	public void AddItemToHotbar(int i, InventoryObject inventoryObject){
		OnAddItemToHotbar?.Invoke(this, new OnAddItemToHotbarEventArgs {
			i = i,
			inventoryObject = inventoryObject
		});
	}

	public void AddItemToInventory(int i, InventoryObject inventoryObject){
		OnAddItemToInventory?.Invoke(this, new OnAddItemToInventoryEventArgs {
			i = i,
			inventoryObject = inventoryObject
		});
	}
}
