using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class TerminalUI : MonoBehaviour
{
    [SerializeField] private TMP_Text terminalDisplay;
    
    private string input = "";
	
    private Executable<string, string> BinaryToDecimal;
    private Folder<string, string> Executables;
    private Folder<string, string> LevelOneRootTerminal;

    private Folder<string, string> currentDirectory;

    private List<string> filePath = new List<string> {"C:", "Users", "root"};
    

    private string baseText = "Keptin Terminal\nCopyright (C) Keptin Corporation. All rights reserved.\n\n";
    
	private bool getInputForExecutable = false;
	private Executable<string, string> runningExecutable;

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
				

			},
			"Please enter a binary number:\n\n"

		);

		Executable<string, string> TestF = new Executable<string, string>(
			"belq.exe",
			(string x) => {return "";},
			"fart"
		);

		Executables = new Folder<string, string>(
			new List<Folder<string, string>> {},
			new List<Executable<string, string>> { BinaryToDecimal, TestF },
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
		input = input.ToLower();
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
					else if(keyCode == KeyCode.Tab){
						HandleAutofill();
					}
					else if(keyCode >= KeyCode.Alpha0 && keyCode <= KeyCode.Alpha9){
						input += (keyCode - KeyCode.Alpha0).ToString();
					}
					else if(keyCode == KeyCode.Return){
						if(getInputForExecutable){
							GetRunningExecutableOutput();
							getInputForExecutable = false;
						}
						else {
							EnterCommand();
						}
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
						getInputForExecutable = true;
						runningExecutable = Exe;
						baseText = Exe.GetInputMessage();
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
				baseText += Folder.GetName() + "\n";
			}
			foreach(Executable<string, string> Executable in currentDirectory.GetExecutables()){
				baseText += Executable.GetName() + "\n";
			}
			baseText += "\n";
		}
		else {
			baseText = "Invalid Command\n\n";
		}
		
		input = "";

    }
	
    private void SetBaseText(){
		terminalDisplay.text = baseText;
		if(!getInputForExecutable){
			foreach(string File in filePath){
				terminalDisplay.text += File + "/";
			}
			terminalDisplay.text = terminalDisplay.text.Substring(0, terminalDisplay.text.Length-1);
			
		}
		terminalDisplay.text += ">";
    }

	private void GetRunningExecutableOutput(){
		input = input.ToLower();
		Func<string, string> inputFunction = runningExecutable.GetFunction();
		baseText = inputFunction(input) + "\n\n";
		getInputForExecutable = false;
		input = "";
	}

	private void HandleAutofill(){
		if(input.Length == 2){
			if(input.Substring(0, 2) == "./"){
				AutofillTerminalInput("./");
			}
		}
		else {
			if(input.Substring(0, 3) == "cd "){
				AutofillTerminalInput("cd ");
			}
			else if(input.Substring(0, 2) == "./"){
				AutofillTerminalInput("./");
			}
		}
	}

	private void AutofillTerminalInput(string command){

		List<GenericFile> AutofillList = new List<GenericFile>();
		string strippedInput = input;

		if(command.Length < input.Length){
			strippedInput = input.Substring(command.Length);

			foreach(Folder<string, string> folder in currentDirectory.GetFolders()){
				if(strippedInput == folder.GetName().Substring(0, strippedInput.Length)){
					AutofillList.Add(folder);
				}
			}
			foreach(Executable<string, string> Exe in currentDirectory.GetExecutables()){
				if(strippedInput == Exe.GetName().Substring(0, strippedInput.Length)){
					AutofillList.Add(Exe);
				}
			}

			if(AutofillList.Count == 1){
				input = command + AutofillList[0].GetName();
			} else {

				baseText = "";
				foreach(GenericFile File in AutofillList){
					baseText += File.GetName() + "\n";
				}
				baseText += "\n";
			}
				
		}

	}

}
