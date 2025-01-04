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

    private Folder<string, string> currentDirectory;

    private List<string> filePath = new List<string> {"C:", "Users", "root"};
    

    private string baseText = "Keptin Terminal\nCopyright (C) Keptin Corporation. All rights reserved.\n\n";
    

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
		new List<Executable<string, string>> { BinaryToDecimal },
		"executables"

    	);

    	LevelOneRootTerminal = new Folder<string, string>(
		new List<Folder<string, string>> { Executables },
		new List<Executable<string, string>> {},
		"root"
	);

	Executables.SetParentFolder(LevelOneRootTerminal);
	

    	SetBaseText();
	currentDirectory = LevelOneRootTerminal;
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
				else if(keyCode == KeyCode.Return){
					EnterCommand();
				}
			}
		}
	}
	if(keyPressed){
		SetBaseText();
		terminalDisplay.text += input.ToLower();
		
	}
    }

    private void EnterCommand(){
	input = input.ToLower();
	Debug.Log(input);
	if(input.Length >= 3){
		if(input.Substring(0,3) == "cd "){
			string folderName = input.Substring(3);
			if(folderName == "..") {
				if(currentDirectory.GetParentFolder() != null){
					currentDirectory = currentDirectory.GetParentFolder();
					baseText = "";
					filePath.RemoveAt(filePath.Count - 1);
				}
			}
			else {
				bool foundFolder = false;
				foreach(Folder<string, string> Folder in currentDirectory.GetFolders()){
					if(folderName == Folder.GetName()){
						currentDirectory = Folder;
						baseText="";			
						filePath.Add(Folder.GetName());
						foundFolder = true;
					}
				}
				if(!foundFolder){
					baseText = "Invalid Subdirectory\n\n";
				}
			}
			
			
		}
		else if(input.Substring(0,2) == "./"){
			string executableName = input.Substring(2);
			foreach(Executable<string, string> Exe in currentDirectory.GetExecutables()){
				if(executableName == Exe.GetName()){
					//run file
				}
			}
		}
		else {
			baseText = "Invalid Command\n\n";
		}
	}
	else if(input == "ls"){
		baseText = "";
		foreach(Folder<string, string> Folder in currentDirectory.GetFolders()){
			baseText += Folder.GetName() + "/\n";
		}
		foreach(Executable<string, string> Executable in currentDirectory.GetExecutables()){
			baseText += Executable.GetName() + "\n";
		}
		baseText += "\n";
	}
	
	input = "";

    }
	
    public void SetBaseText(){
	terminalDisplay.text = baseText;
	foreach(string File in filePath){
		terminalDisplay.text += File + "/";
	}
	terminalDisplay.text = terminalDisplay.text.Substring(0, terminalDisplay.text.Length-1);
	terminalDisplay.text += ">";
    }
}
