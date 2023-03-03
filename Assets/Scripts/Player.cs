using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance {
        get; private set; }


    //serializedFields show up in the editor, but do not become public
    [SerializeField] private float playerMoveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    private bool isWalking;
    private Vector3 lastIneractDir;
    private ClearCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There Is more than one player instance");
        }
        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
        
    }

    private void HandleInteractions()
    {
        float interactDistance = 2f;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastIneractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastIneractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {

                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
                
                /*else
                {
                    if (clearCounter == selectedCounter)
                    {
                        //do nothing
                    }
                    else
                    {
                        if (clearCounter == null)
                        {
                            SetSelectedCounter(null);
                        }
                    }
                    
                }
            */
            }

            else
            {
                SetSelectedCounter(null);
            }
        }  
        else
        {
            SetSelectedCounter(null);
        }
    
    }

    private void HandleMovement()
    {
        float playerRadius = 0.7f;
        float playerHeight = 2.0f;
        float moveDistance = playerMoveSpeed * Time.deltaTime;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDir, moveDistance);
        {
            if (!canMove)
            {
                // cannot move towards moveDir

                //Attempt only X movement
                Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirX, moveDistance);
                if (canMove)
                {
                    //canMove only in X Dir
                    moveDir = moveDirX;
                }
                else
                {
                    //cannot move only on X 

                    //Attempt Z only movement
                    Vector3 moveDirZ = new Vector3(moveDir.z, 0f, 0f).normalized;
                    canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHeight), playerRadius, moveDirZ, moveDistance);

                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                    else
                    {
                        //Cannot move in either direction

                    }
                }

            }



            if (canMove)
            {
                transform.position += moveDir * Time.deltaTime * playerMoveSpeed;
                isWalking = moveDir != Vector3.zero;
            }
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);


            
        }

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
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
