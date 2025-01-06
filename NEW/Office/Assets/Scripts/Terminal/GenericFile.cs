using UnityEngine;

public class GenericFile 
{
	private string name;

	public GenericFile(string name){
		this.name = name;
	}

	public string GetName(){
		return name;
	}
}

