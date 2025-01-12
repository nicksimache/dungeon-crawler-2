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

	public event EventHandler<OnAccessTerminalEventArgs> OnAccessTerminal;
	public class OnAccessTerminalEventArgs : EventArgs {
		public bool openTerminal;
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
}
