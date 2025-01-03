using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class TerminalUI : MonoBehaviour
{
    [SerializeField] private TMP_Text terminalDisplay;
    
    private string input;


    private Executable<string, string> BinaryToDecimal;
    private Folder<string, string> Executables;
    private Folder<string, string> LevelOneRootTerminal;

    private string baseText = "Keptin Terminal\nCopyright (C) Keptin Corporation. All rights reserved.\n\nC:/Users/root>";
    

    private void Start() {
	BinaryToDecimal = new Executable<string, string>(
		"binarytodecimal.exe",
		(string binaryString) => {
			int decimalValue = 0;
       			int baseValue = 1;

        
        		for (int i = binaryString.Length - 1; i >= 0; i--){
            			char c = binaryString[i];
            
           	   		if (c != '0' && c != '1'){
               		 		return "Invalid Input";
            			}

            
            		int bit = c - '0';
            		decimalValue += bit * baseValue;
            		baseValue *= 2;
        		}

			return decimalValue.ToString();
        	

		}
    	);

    	Executables = new Folder<string, string>(
		new List<Folder<string, string>> {},
		new List<Executable<string, string>> { BinaryToDecimal }

    	);

    	LevelOneRootTerminal = new Folder<string, string>(
		new List<Folder<string, string>> { Executables },
		new List<Executable<string, string>> {}

	);
	

    	terminalDisplay.text = baseText;
    }

    private bool keyPressed = false;

    private void Update() {
	keyPressed = false;
	if(Input.anyKeyDown){
		keyPressed = true;
		foreach(KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))){
			if(Input.GetKeyDown(keyCode)){
				if(keyCode == KeyCode.Backspace){
					if(!string.IsNullOrEmpty(input)){
						input = input.Substring(0, input.Length - 1);
					}
				}
				else if(keyCode >= KeyCode.A && keyCode <= KeyCode.Z){
					input += keyCode;
				}
				else if(keyCode == KeyCode.Space){
					input += " ";
				}
				else if(keyCode == KeyCode.Period){
					input += ".";
				}
				else if(keyCode == KeyCode.Slash){
					input += "/";
				}
			}
		}
	}
	if(keyPressed){
		terminalDisplay.text = baseText + input.ToLower();
		
	}
    }

}
