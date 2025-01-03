using UnityEngine;
using UnityEngine.UI;
using System;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private InteractObject interactObject;
    [SerializeField] private Image barImage;



    private void Start() {
	interactObject.OnInteractProgressChanged += InteractObject_OnInteractProgressChanged;	
	interactObject.OnResetProgressBar += InteractObject_OnResetProgressBar;

	

    }

    private void InteractObject_OnInteractProgressChanged(object sender, ComputerObject.OnInteractProgressChangedEventArgs e) {
	barImage.fillAmount = e.progressNormalized;
    }

    private void InteractObject_OnResetProgressBar(object sender, EventArgs e){
	barImage.fillAmount = 0f;
    }    
}
