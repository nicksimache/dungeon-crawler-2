using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


public class Player : MonoBehaviour {
	
	public static Player Instance { get; private set; } 

	[SerializeField] private GameInput gameInput;

	[SerializeField] private Image toolbar;
	public List<InventoryObject> playerInventoryObjectList = new List<InventoryObject>();
	private List<Image> hotbarImages = new List<Image>(); // list of hotbar slots (images)

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
	

	private bool canMove = true;

	[SerializeField] private LayerMask interactLayerMask;
	private InteractObject selectedObject;

	public event EventHandler<OnSelectedObjectChangedEventArgs> OnSelectedObjectChanged;
	public class OnSelectedObjectChangedEventArgs : EventArgs {
		public InteractObject selectedObject;
		public float distance;
	}


	private void Awake(){
		if(Instance != null){
			Debug.LogError("Already have an instance of a player");
		}
		Instance = this;

		foreach(Transform child in toolbar.transform){
			hotbarImages.Add(child.GetComponent<Image>());
		}
	}

    public void Start() {
		gameInput.OnInteractAction += GameInput_OnInteractAction;
		gameInput.OnStopInteract += GameInput_OnStopInteract;
		gameInput.OnCloseTerminal += GameInput_OnCloseTerminal;

		EventManager.Instance.OnAccessTerminal += EventManager_OnAccessTerminal;

		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		EventManager.Instance.OnPickUpItem += EventManager_OnPickUpItem;
    }

	void Update() {
		FillHotbar();
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

	private void FillHotbar(){
		for(int i = 0 ; i < Mathf.Min(hotbarImages.Count, playerInventoryObjectList.Count); i++){
			if(playerInventoryObjectList != null){
				hotbarImages[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
				hotbarImages[i].transform.GetChild(0).GetComponent<Image>().sprite = playerInventoryObjectList[i].GetInventoryObjectSprite();

			}
		}
	}

	public GameObject lastHit;
	private void HandleInteractions(){
		Vector3 lookDirection = playerCamera.transform.forward;
		float interactRange = 2f;

		if(Physics.Raycast(playerCamera.transform.position, lookDirection, out RaycastHit raycastHit, interactRange, interactLayerMask)){
			lastHit = raycastHit.transform.gameObject;
			float raycastHitDistance = raycastHit.distance;
			if(raycastHit.transform.parent.parent.TryGetComponent(out InteractObject interactObject)){
				lastHit = interactObject.gameObject;
				if(interactObject != selectedObject){
					SetSelectedObject(interactObject, raycastHitDistance);
				}
			}
			else{
				SetSelectedObject(null, 0);
			}
		}
		else {
			SetSelectedObject(null, 0);
		}


	}

	private void SetSelectedObject(InteractObject interactObject, float distance){
		this.selectedObject = interactObject;

		OnSelectedObjectChanged?.Invoke(this, new OnSelectedObjectChangedEventArgs {
			selectedObject = selectedObject,
			distance = distance
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

    private void EventManager_OnAccessTerminal(object sender, EventManager.OnAccessTerminalEventArgs e){
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

    public Vector3 GetPlayerLookDirNormalized(){
		return playerCamera.transform.forward.normalized;
    }

    public Vector3 GetPlayerCameraPos(){
		return playerCamera.transform.position;
    }

	public int GetNumInventoryObjects(){
		return playerInventoryObjectList.Count;
	}

}
