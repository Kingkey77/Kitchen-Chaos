using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
       playerInputActions =  new PlayerInputActions();
       playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
    
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        //Debug.Log(obj);
        //Debug.Log("key Pressed");
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        //The above line does the following code in a single line. 
        //if (OnInteractAction != null)                         //checks for subscribers and makes sure we do not get a null reference 
        //{
        //    OnInteractAction(this, EventArgs.Empty);
        //}
        
        
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
