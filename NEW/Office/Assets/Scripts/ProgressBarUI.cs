using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private InteractObject interactObject;
    [SerializeField] private Image barImage;

    [SerializeField] private List<GameObject> visualGameObjectList = new List<GameObject>();

    private void Start() {
		interactObject.OnInteractProgressChanged += InteractObject_OnInteractProgressChanged;	
		interactObject.OnResetProgressBar += InteractObject_OnResetProgressBar;

		Player.Instance.OnSelectedObjectChanged += Player_OnSelectedObjectChanged;

    }

    private void Player_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e){
	if(e.selectedObject == interactObject){
		foreach(GameObject gameObject in visualGameObjectList){
			gameObject.SetActive(true);
		}
	}
	else {
		foreach(GameObject gameObject in visualGameObjectList){
			gameObject.SetActive(false);
		}
	}
	
    }

    private void InteractObject_OnInteractProgressChanged(object sender, ComputerObject.OnInteractProgressChangedEventArgs e) {
		barImage.fillAmount = e.progressNormalized;
    }

    private void InteractObject_OnResetProgressBar(object sender, EventArgs e){
		barImage.fillAmount = 0f;
    }    
}
