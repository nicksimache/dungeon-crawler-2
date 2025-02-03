using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GunItem", menuName = "Scriptable Objects/GunItem")]
public class GunItem : _BaseItem
{

    public void Shoot(){
        Debug.Log("pew");
    }
}
