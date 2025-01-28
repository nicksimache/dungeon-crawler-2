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
    }

	public event EventHandler<OnInteractInventoryEventArgs> OnInteractInventory;
	public class OnInteractInventoryEventArgs : EventArgs {
		public bool openInventory;
	}

	public event EventHandler<OnAccessTerminalEventArgs> OnAccessTerminal;
	public class OnAccessTerminalEventArgs : EventArgs {
		public bool openTerminal;
	}

	public event EventHandler<OnOpenChestEventArgs> OnOpenChest;
	public class OnOpenChestEventArgs : EventArgs {
		public bool openChest;
	}

    public void PickUpItem(InventoryObject inventoryObject){
		OnPickUpItem?.Invoke(this, new OnPickUpItemEventArgs {
			inventoryObject = inventoryObject
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

	public void OpenInvetory(bool openInventory){
		OnInteractInventory?.Invoke(this, new OnInteractInventoryEventArgs{
			openInventory = openInvetory;
		})
	}
}
