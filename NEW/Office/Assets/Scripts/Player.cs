using UnityEngine;
using System;
using System.Collections.Generic;


public class Player : MonoBehaviour {
	
	public static Player Instance { get; private set; } 

	[SerializeField] private GameInput gameInput;
    	[SerializeField] private ComputerObject tempObj;

	[SerializeField] public Camera playerCamera;
    	[SerializeField] public float walkSpeed = 6f;
    	[SerializeField] public float jumpPower = 7f;
    	[SerializeField] public float gravity = 10f;
    	[SerializeField] public float lookSpeed = 2f;
    	[SerializeField] public float lookXLimit = 45f;
    	[SerializeField] public float defaultHeight = 2f;
    	[SerializeField] public float crouchHeight = 1f;
    	[SerializeField] public float crouchSpeed = 3f;
	
	private Vector3 moveDirection = Vector3.zero;
	private float rotationX = 0;
	private CharacterController characterController;
	
	public List<InventoryObject> playerInventoryObjectList;

	private bool canMove = true;

	[SerializeField] private LayerMask interactLayerMask;
	private InteractObject selectedObject;

	public event EventHandler<OnSelectedObjectChangedEventArgs> OnSelectedObjectChanged;
	public class OnSelectedObjectChangedEventArgs : EventArgs {
		public InteractObject selectedObject;
	}


	private void Awake(){
		if(Instance != null){
			Debug.LogError("Already have an instance of a player");
		}
		Instance = this;
	}

    public void Start() {
		gameInput.OnInteractAction += GameInput_OnInteractAction;
		gameInput.OnStopInteract += GameInput_OnStopInteract;
		gameInput.OnCloseTerminal += GameInput_OnCloseTerminal;

		tempObj.OnAccessTerminal += ComputerObject_OnAccessTerminal;

		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		EventManager.Instance.OnPickUpItem += EventManager_OnPickUpItem;
    }

	void Update()
    {
		HandleInteractions();
		
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? walkSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? walkSpeed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

	public GameObject lastHit;
	private void HandleInteractions(){
		Vector3 lookDirection = playerCamera.transform.forward;
		float interactRange = 2f;

		if(Physics.Raycast(playerCamera.transform.position, lookDirection, out RaycastHit raycastHit, interactRange, interactLayerMask)){
			lastHit = raycastHit.transform.gameObject;
			if(raycastHit.transform.parent.parent.TryGetComponent(out InteractObject interactObject)){
				if(interactObject != selectedObject){
					SetSelectedObject(interactObject);
				}
			}
			else{
				SetSelectedObject(null);
			}
		}
		else {
			SetSelectedObject(null);
		}


	}

	private void SetSelectedObject(InteractObject interactObject){
		this.selectedObject = interactObject;

		OnSelectedObjectChanged?.Invoke(this, new OnSelectedObjectChangedEventArgs {
			selectedObject = selectedObject
		});
	}

    private void GameInput_OnInteractAction (object sender, EventArgs e){
	if(selectedObject != null){
		selectedObject.Interact(this);
	}
    }

    private void GameInput_OnStopInteract (object sender, EventArgs e){
	if(selectedObject != null){
		selectedObject.ResetProgressBar();
	}
    }

    private void GameInput_OnCloseTerminal(object sender, EventArgs e){
	if(selectedObject != null){
		if(selectedObject.TryGetComponent(out ComputerObject computerObject)){
			computerObject.CloseTerminal();
		}
	}
    }

    private void ComputerObject_OnAccessTerminal(object sender, ComputerObject.OnAccessTerminalEventArgs e){
		if(e.openTerminal){
			canMove = false;
		}
		else {
			canMove = true;
		}
    }

    private void EventManager_OnPickUpItem(object sender, EventManager.OnPickUpItemEventArgs e){
	playerInventoryObjectList.Add(e.inventoryObject);
    }

}
