using UnityEngine;
using System;
using System.Collections.Generic;

public class Folder<T, TReturn> : GenericFile
{
    private List<Folder<T, TReturn>> Folders;
    private List<Executable<T, TReturn>> Executables;
    private Folder<T, TReturn> ParentFolder;	
    
    public Folder(List<Folder<T, TReturn>> Folders, List<Executable<T, TReturn>> Executables) : base(""){
	this.Folders = Folders;
	this.Executables = Executables;
	this.ParentFolder = null;
    }

    public Folder(List<Folder<T, TReturn>> Folders, List<Executable<T, TReturn>> Executables, string name) : base(name){
	this.Folders = Folders;
	this.Executables = Executables;
	this.ParentFolder = null;
    }

    public Folder(List<Folder<T, TReturn>> Folders, List<Executable<T, TReturn>> Executables, string name, Folder<T, TReturn> ParentFolder) : base(name){
	this.Folders = Folders;
	this.Executables = Executables;
	this.ParentFolder = ParentFolder;
    }

    public List<Folder<T, TReturn>> GetFolders (){
	return Folders;
    }

    public void SetFolders (List<Folder<T, TReturn>> Folders) {
	this.Folders = Folders;
    }

    public List<Executable<T, TReturn>> GetExecutables (){
	return Executables;
    }

    public void SetExecutables (List<Executable<T, TReturn>> Executables){
	this.Executables = Executables;
    }

    public Folder<T, TReturn> GetParentFolder (){
	if(ParentFolder == null){
		return null;
	}
	return ParentFolder;
    }

    public void SetParentFolder(Folder<T, TReturn> ParentFolder){
	this.ParentFolder = ParentFolder;
    }


}
