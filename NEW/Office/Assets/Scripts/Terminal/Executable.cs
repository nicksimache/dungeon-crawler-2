using UnityEngine;
using System;
using System.Collections.Generic;
public class Executable<T, TReturn> 
{
    private string name {get; set;}
    private Func<T, TReturn> Function {get; set;}
	private string inputMessage;
    public Executable(string name, Func<T, TReturn> Function, string inputMessage){
		this.name = name;
		this.Function = Function;
		this.inputMessage = inputMessage;
    }

    public string GetName(){
		return name;
    }

	public Func<T, TReturn> GetFunction(){
		return Function;
	}

	public string GetInputMessage(){
		return inputMessage;
	}
    
}
