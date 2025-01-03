using UnityEngine;
using System;
using System.Collections.Generic;
public class Executable<T, TReturn> 
{
    private string name {get; set;}
    private Func<T, TReturn> Function {get; set;}

    public Executable(string name, Func<T, TReturn> Function){
	this.name = name;
	this.Function = Function;
    }
    
}
