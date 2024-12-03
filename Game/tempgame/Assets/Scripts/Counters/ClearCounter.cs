using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ClearCounter : BaseCounter
{


    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private const string TEMP_COUNTER = "TempCounter";





    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if(player.HasKitchenObject())
            {
                if(player.GetKitchenObject() is PlateKitchenObject)
                {
                    PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    plateKitchenObject.AddIngredient(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                }
                else
                {
                    ClearCounter tempClearCounter = GameObject.FindWithTag(TEMP_COUNTER).GetComponent<ClearCounter>();
                    GetKitchenObject().SetKitchenObjectParent(tempClearCounter);
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    tempClearCounter.GetKitchenObject().SetKitchenObjectParent(player);
                }

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

   
}