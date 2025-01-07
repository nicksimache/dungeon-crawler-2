using UnityEngine;
using UnityEngine.UI;
using System;

public class ComputerObject : InteractObject
{
	private Transform terminalUI;
	private Transform progressBarUI;
	private string TERMINAL_UI = "TerminalUI";
	private string PROGRESSBARUI = "ProgressBarUI";

	public event EventHandler<OnAccessTerminalEventArgs> OnAccessTerminal;
	public class OnAccessTerminalEventArgs : EventArgs {
		public bool openTerminal;
	}

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

			OnAccessTerminal?.Invoke(this, new OnAccessTerminalEventArgs{
				openTerminal = true
			});
		}
		
	}

	public void CloseTerminal(){
		if(!canInteract){
			terminalUI.gameObject.SetActive(false);
			canInteract = true;
			progressBarUI.gameObject.SetActive(true);

			OnAccessTerminal?.Invoke(this, new OnAccessTerminalEventArgs{
				openTerminal = false
			});
		}
	}
    
}
