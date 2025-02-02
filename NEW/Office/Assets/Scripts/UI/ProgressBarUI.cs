using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ProgressBarUI : MonoBehaviour
{
	private Animator animator;

    private const string POP_UP = "PopUp";
    private const string PROGRESS_BAR_IDLE = "ProgressBarUIIdle";


    [SerializeField] private InteractObject interactObject;
    [SerializeField] private Image barImage;

    [SerializeField] private List<GameObject> visualGameObjectList = new List<GameObject>();

    private bool progressBarVisible = false;

	private void Awake(){
        animator = GetComponent<Animator>();
    }


    private void Start() {
		interactObject.OnInteractProgressChanged += InteractObject_OnInteractProgressChanged;	
		interactObject.OnResetProgressBar += InteractObject_OnResetProgressBar;

		//Player.Instance.OnSelectedObjectChanged += Player_OnSelectedObjectChanged;

    }

	private InteractObject lastSelectedObject;
    private void Player_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e)
{
		if (e.selectedObject == interactObject)
		{
			foreach (GameObject gameObject in visualGameObjectList)
			{
				gameObject.SetActive(true);
			}

			animator.Play(POP_UP);
			interactObject.MakeCanInteract(true);

		}
		else
		{
			foreach (GameObject gameObject in visualGameObjectList)
			{
				gameObject.SetActive(false);
			}

			animator.Play(PROGRESS_BAR_IDLE);
			interactObject.MakeCanInteract(false);
		}

		lastSelectedObject = e.selectedObject;
	}
		
    

    private void InteractObject_OnInteractProgressChanged(object sender, ComputerObject.OnInteractProgressChangedEventArgs e) {
		barImage.fillAmount = e.progressNormalized;
    }

    private void InteractObject_OnResetProgressBar(object sender, EventArgs e){
		barImage.fillAmount = 0f;
    }    
	
    private string GetAnimationNameFromHash(int hash)
    {
        if (hash == Animator.StringToHash("PopUp"))
            return "PopUp";
        if (hash == Animator.StringToHash("ProgressBarUIIdle"))
            return "ProgressBarIdle";

        return "ProgressBarHidden";
    }
}
