using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerminalUI : MonoBehaviour
{
    [SerializeField] private TMP_Text terminalDisplay;
    
    private string input;

    private void Start(){
	terminalDisplay.text = "";
    }

    private void Update() {
	if(Input.anyKeyDown){
		foreach(KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))){
			if(Input.GetKeyDown(keyCode)){
				if(keyCode == KeyCode.Backspace){
					if(!string.IsNullOrEmpty(input)){
						input = input.Substring(0, input.Length - 1);
					}
				}
				else if(keyCode >= KeyCode.A && keyCode <= KeyCode.Z){
					input += keyCode;
					Debug.Log(input);
				}
			}
		}
	}

	terminalDisplay.text = input;
    }

}
