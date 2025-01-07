using UnityEngine;

public class InteractObjectSelectedVisual : MonoBehaviour
{
	[SerializeField] InteractObject interactObject;
	[SerializeField] GameObject visualGameObject;

	private void Start(){
		Player.Instance.OnSelectedObjectChanged += Player_OnSelectedObjectChanged;
	}

	private void Player_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e) {
		if(e.selectedObject == interactObject){
			visualGameObject.SetActive(true);
			Debug.Log("show");
		}
		else {
			visualGameObject.SetActive(false);
		}
	}

}
