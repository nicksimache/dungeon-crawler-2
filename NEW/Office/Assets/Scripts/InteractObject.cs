using UnityEngine;
using System;

public class InteractObject : MonoBehaviour
{
	public event EventHandler<OnInteractProgressChangedEventArgs> OnInteractProgressChanged;
	public class OnInteractProgressChangedEventArgs: EventArgs {
		public float progressNormalized;
	}

	public event EventHandler OnResetProgressBar;

	protected float interactProgress = 0f;
	protected float interactProgressMax = 1.0f;
	protected bool canInteract = true;
	
	public void Interact(Player player) {
		if(canInteract){
			interactProgress += Time.deltaTime;

			interactProgress = Mathf.Clamp(interactProgress, 0f, interactProgressMax);

			OnInteractProgressChanged?.Invoke(this, new OnInteractProgressChangedEventArgs{
				progressNormalized = interactProgress / interactProgressMax
			});
		}
		
	}

	public void ResetProgressBar() {
		OnResetProgressBar?.Invoke(this, EventArgs.Empty);
		interactProgress = 0f;
	}
}
