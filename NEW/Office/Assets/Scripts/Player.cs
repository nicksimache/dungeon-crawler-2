using UnityEngine;
using System;


public class Player : MonoBehaviour {

    [SerializeField] private GameInput gameInput;
    [SerializeField] private ComputerObject tempObj;
	
    public void Start() {
	gameInput.OnInteractAction += GameInput_OnInteractAction;
	gameInput.OnStopInteract += GameInput_OnStopInteract;
	gameInput.OnCloseTerminal += GameInput_OnCloseTerminal;
    }

    private void GameInput_OnInteractAction (object sender, EventArgs e){
	tempObj.Interact(this);
    }

    private void GameInput_OnStopInteract (object sender, EventArgs e){
	tempObj.ResetProgressBar();
    }

    private void GameInput_OnCloseTerminal(object sender, EventArgs e){
	tempObj.CloseTerminal();
    }

}
