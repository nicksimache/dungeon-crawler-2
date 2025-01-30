using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

	public event EventHandler OnInteractAction;
	public event EventHandler OnStopInteract;
	public event EventHandler OnCloseTerminal;
	public event EventHandler<OnOpenInventoryEventArgs> OnOpenInventory;
	public class OnOpenInventoryEventArgs : EventArgs {
		public bool playerClosedInventory;
	}

	public event EventHandler OnCloseChest;


	public event EventHandler<OnSwitchHotbarSelectedItemEventArgs> OnSwitchHotbarSelectedItem;
	public class OnSwitchHotbarSelectedItemEventArgs : EventArgs {
		public int inventorySlot;
	}

	private InputSystem_Actions inputSystemActions;



	private void Awake() {
		inputSystemActions = new InputSystem_Actions();
		inputSystemActions.Player.Enable();

		inputSystemActions.Player.InventorySlot1.performed += _ => SwitchHotbarSlot(0);
        inputSystemActions.Player.InventorySlot2.performed += _ => SwitchHotbarSlot(1);
        inputSystemActions.Player.InventorySlot3.performed += _ => SwitchHotbarSlot(2);
        inputSystemActions.Player.InventorySlot4.performed += _ => SwitchHotbarSlot(3);
        inputSystemActions.Player.InventorySlot5.performed += _ => SwitchHotbarSlot(4);
		inputSystemActions.Player.OpenInventory.performed += _ => OpenInventory();
		inputSystemActions.Player.CloseTerminal.performed += _ => HandleEscPress();
	}

	public bool IsPlayerInteracting() {
		return (inputSystemActions.Player.Interact.ReadValue<float>() > 0.0);
	}

	private void Update() {
		if(IsPlayerInteracting()){
			OnInteractAction?.Invoke(this, EventArgs.Empty);
		}
		else {
			OnStopInteract?.Invoke(this, EventArgs.Empty);
		}
				
	}

	private void HandleEscPress(){
		OnCloseTerminal?.Invoke(this, EventArgs.Empty);
		OnOpenInventory?.Invoke(this, new OnOpenInventoryEventArgs{
			playerClosedInventory = true
		});	}

	private void SwitchHotbarSlot(int slot)
    {
        OnSwitchHotbarSelectedItem?.Invoke(this, new OnSwitchHotbarSelectedItemEventArgs
        {
            inventorySlot = slot
        });
    }

	private void OpenInventory(){
		OnOpenInventory?.Invoke(this, new OnOpenInventoryEventArgs{
			playerClosedInventory = false
		});
	}

	private void OnDestroy(){
        inputSystemActions.Player.InventorySlot1.performed -= _ => SwitchHotbarSlot(0);
        inputSystemActions.Player.InventorySlot2.performed -= _ => SwitchHotbarSlot(1);
        inputSystemActions.Player.InventorySlot3.performed -= _ => SwitchHotbarSlot(2);
        inputSystemActions.Player.InventorySlot4.performed -= _ => SwitchHotbarSlot(3);
        inputSystemActions.Player.InventorySlot5.performed -= _ => SwitchHotbarSlot(4);
		inputSystemActions.Player.OpenInventory.performed -= _ => OpenInventory();
		inputSystemActions.Player.CloseTerminal.performed -= _ => HandleEscPress();

	}


}
