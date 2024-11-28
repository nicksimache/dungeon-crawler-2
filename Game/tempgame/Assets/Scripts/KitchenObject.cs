using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{


    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    private ClearCounter clearCounter;


    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        if (this.clearCounter != null)
        {
            this.clearCounter.ClearKitchenObject();
        }

        this.clearCounter = clearCounter;

        try
        {
            Debug.Log(clearCounter == null);
            if (clearCounter.HasKitchenObject())
            {
                Debug.LogError("Counter already has a KitchenObject!");
            }
        }
        catch
        {
            Debug.LogError("IDK");
        }

        clearCounter.SetKitchenObject(this);

        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }



}