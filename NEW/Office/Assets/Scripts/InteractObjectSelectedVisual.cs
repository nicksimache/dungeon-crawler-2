using UnityEngine;

public class InteractObjectSelectedVisual : MonoBehaviour
{
	[SerializeField] private InteractObject interactObject;
	[SerializeField] private GameObject visualGameObject;

	private void Start(){
		//Player.Instance.OnSelectedObjectChanged += Player_OnSelectedObjectChanged;
	}

	private void Player_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e) {
		if(e.selectedObject == interactObject){
			visualGameObject.SetActive(true);
		}
		else {
			visualGameObject.SetActive(false);
		}
	}

}
