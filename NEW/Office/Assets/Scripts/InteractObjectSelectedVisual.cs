using UnityEngine;

public class InteractObjectSelectedVisual : MonoBehaviour
{
	[SerializeField] private InteractObject interactObject;
	[SerializeField] private GameObject visualGameObject;

	private void Start(){
		EventManager.Instance.OnSelectedObjectChanged += EventManager_OnSelectedObjectChanged;
	}

	private void EventManager_OnSelectedObjectChanged(object sender, EventManager.OnSelectedObjectChangedEventArgs e) {
		if(e.selectedObject == interactObject){
			visualGameObject.SetActive(true);
		}
		else {
			visualGameObject.SetActive(false);
		}
	}

}
