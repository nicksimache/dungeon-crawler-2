using UnityEngine;
using System;

public class InteractObject : MonoBehaviour
{

	private float interactProgress = 0f;
	private float interactProgressMax = 1.0f;

	public event EventHandler<OnInteractProgressChangedEventArgs> OnInteractProgressChanged;
	public class OnInteractProgressChangedEventArgs : EventArgs {
		public float progressNormalized;
	}

	public event EventHandler OnResetProgressBar;

	public void Interact(Player player) {

		interactProgress += Time.deltaTime;

		interactProgress = Mathf.Clamp(interactProgress, 0f, interactProgressMax);

		OnInteractProgressChanged?.Invoke(this, new OnInteractProgressChangedEventArgs {
			progressNormalized = interactProgress / interactProgressMax
		});
	}

	public void ResetProgressBar(){
		OnResetProgressBar?.Invoke(this, EventArgs.Empty);
		interactProgress = 0f;
	}
}
