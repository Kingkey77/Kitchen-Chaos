using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform countertopPoint;

    

    private KitchenObject kitchenObject;

    

    public void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            //Debug.Log("Interact!");
            Instantiate(kitchenObjectSO, countertopPoint);
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, countertopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            
            //kitchenObjectTransform.localPosition = Vector3.zero;
            //Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
            //kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            //kitchenObject.SetClearCounter(this);
        }
        else
        {
            //Give the object to the player
            kitchenObject.SetKitchenObjectParent(player);


            Debug.Log(kitchenObject.GetKitchenObjectParent());
        }

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return countertopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    { 
        return kitchenObject; 
    }

    public void ClearKitchenObject()
    { 
        kitchenObject = null; 
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;   
    }
}
