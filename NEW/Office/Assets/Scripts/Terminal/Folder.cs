using UnityEngine;
using System;
using System.Collections.Generic;

public class Folder<T, TReturn>
{
    private List<Folder<T, TReturn>> Folders {get; set;}
    private List<Executable<T, TReturn>> Executables {get; set;}
	
    private string name;


    public Folder(List<Folder<T, TReturn>> Folders, List<Executable<T, TReturn>> Executables){
	this.Folders = Folders;
	this.Executables = Executables;
	this.name = "";
    }

    public Folder(List<Folder<T, TReturn>> Folders, List<Executable<T, TReturn>> Executables, string name){
	this.Folders = Folders;
	this.Executables = Executables;
	this.name = name;
    }


}
