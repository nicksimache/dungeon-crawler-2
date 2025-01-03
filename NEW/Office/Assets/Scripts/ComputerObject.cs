using UnityEngine;
using UnityEngine.UI;
using System;

public class ComputerObject : InteractObject
{
	private Transform terminalUI;
	private Transform progressBarUI;
	private string TERMINAL_UI = "TerminalUI";
	private string PROGRESSBARUI = "ProgressBarUI";

	private void Start(){
		terminalUI = transform.Find(TERMINAL_UI);
		progressBarUI = transform.Find(PROGRESSBARUI);
	}	

	private void Update(){
		if(interactProgress == interactProgressMax){
			ResetProgressBar();
			terminalUI.gameObject.SetActive(true);
			canInteract = false;
			progressBarUI.gameObject.SetActive(false);
		}

		if(!canInteract){
			
		}
		
	}

	public void CloseTerminal(){
		if(!canInteract){
			terminalUI.gameObject.SetActive(false);
			canInteract = true;
			progressBarUI.gameObject.SetActive(true);
		}
	}
    
}
