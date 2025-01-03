using UnityEngine;
using System;

public class GameInput : MonoBehaviour {

	public event EventHandler OnInteractAction;
	public event EventHandler OnStopInteract;
	public event EventHandler OnCloseTerminal;

	private InputSystem_Actions inputSystemActions;

	private void Awake() {
		inputSystemActions = new InputSystem_Actions();
		inputSystemActions.Player.Enable();
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
		
		if(inputSystemActions.Player.CloseTerminal.ReadValue<float>() > 0.0){
			OnCloseTerminal?.Invoke(this, EventArgs.Empty);
		}
		
	}
}
