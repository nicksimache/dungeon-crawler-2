using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Unity.Netcode;


public class Player : NetworkBehaviour {
	
	public static Player LocalInstance { get; private set; } 

	private Image toolbar;
	private Image inventoryToolbar;
	private InventoryObject[] playerInventoryObjectList = new InventoryObject[5]; // THIS IS FOR THE HOTBAR
	private List<Transform> hotbarImages = new List<Transform>(); // list of hotbar slots (images)

	private InventoryObject[] playerMainInventoryObjectList = new InventoryObject[15];
	private List<Transform> inventoryImages = new List<Transform>();
	private int numItemsInHotbar = 0;
	private int numItemInInventory = 0;


	private int selectedHotbarSlot = -1;
	private bool isInventoryOpen = false;

	private Camera playerCamera;
	[SerializeField] private GameObject mainCameraPrefab;
	private GameObject heldItem = null;

    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 45f;
    [SerializeField] private float defaultHeight = 2f;
   	[SerializeField] private float crouchHeight = 1f;
   	[SerializeField] private float crouchSpeed = 3f;

	[SerializeField] private Vector3 playerItemPosition;
	
	private Vector3 moveDirection = Vector3.zero;
	private float rotationX = 0;
	private CharacterController characterController;	

	private bool canMove = true;
	private bool canMoveCamera = true;

	private bool playerUsingComputer = false;

	[SerializeField] private LayerMask interactLayerMask;
	private InteractObject selectedObject;

    public void Start() {

		toolbar = UIManager.Instance.toolbar;
		inventoryToolbar = UIManager.Instance.inventoryToolbar;

		foreach(Transform child in toolbar.transform){
			hotbarImages.Add(child);
		}
		foreach(Transform child in inventoryToolbar.transform){
			inventoryImages.Add(child);
		}

		if(IsOwner){
			LocalInstance = this;
			InstantiateMainCamera();
		}

		GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
		GameInput.Instance.OnStopInteract += GameInput_OnStopInteract;
		GameInput.Instance.OnCloseTerminal += GameInput_OnCloseTerminal;
		GameInput.Instance.OnCloseChest += GameInput_OnCloseChest;
		GameInput.Instance.OnOpenInventory += GameInput_OnOpenInventory;
		GameInput.Instance.OnSwitchHotbarSelectedItem += GameInput_OnSwitchHotbarSelectedItem;
		GameInput.Instance.OnUseItem += GameInput_OnUseItem;

		EventManager.Instance.OnAccessTerminal += EventManager_OnAccessTerminal;

		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		EventManager.Instance.OnPickUpItem += EventManager_OnPickUpItem;
    }

	void Update() {
		if(!IsOwner){
			return;
		}
		HandleInventory();
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

        if (canMove && canMoveCamera)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

	private void HandleInventory(){
		numItemsInHotbar = 0;
		for(int i = 0; i < 5; i++){
			if(hotbarImages[i].childCount > 0){
				playerInventoryObjectList[i] = hotbarImages[i].GetChild(0).GetComponent<InventoryObjectUI>().GetInventoryObject();
				numItemsInHotbar++;
			}
			else {
				playerInventoryObjectList[i] = null;
			}

			if(i == selectedHotbarSlot){
				hotbarImages[i].transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
				if(playerInventoryObjectList[i] != null && heldItem == null){
					heldItem = Instantiate(playerInventoryObjectList[i].GetInventoryObjectSO().GetItemPrefab(), new Vector3(0,0,0), Quaternion.identity);
					heldItem.transform.SetParent(transform);
					heldItem.transform.localPosition = playerItemPosition;
					heldItem.layer = LayerMask.NameToLayer("Default");

				}
			}
			else {
				hotbarImages[i].transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		numItemInInventory = 0;
		for(int i = 0; i < 15; i++){
			if(inventoryImages[i].childCount > 0){
				playerMainInventoryObjectList[i] = inventoryImages[i].GetChild(0).GetComponent<InventoryObjectUI>().GetInventoryObject();
				numItemInInventory++;
			}
			else {
				playerMainInventoryObjectList[i] = null;
			}
		}
	}

	private void InstantiateMainCamera(){
		Debug.Log("Main Camera instnatiasdasdasdsa");
		GameObject go = Instantiate(mainCameraPrefab, new Vector3(0,0,0), Quaternion.identity);
		playerCamera = go.GetComponent<Camera>();
		playerCamera.transform.SetParent(gameObject.transform);
		playerCamera.transform.localPosition = Vector3.zero;
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

		EventManager.Instance.SelectedObjectChanged(selectedObject, distance);
	}

	private void GameInput_OnSwitchHotbarSelectedItem (object sender, GameInput.OnSwitchHotbarSelectedItemEventArgs e){
		if(selectedHotbarSlot != e.inventorySlot){
			selectedHotbarSlot = e.inventorySlot;
		}
		else {
			selectedHotbarSlot = -1;
		}
		Destroy(heldItem);
		heldItem = null;
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

	private void GameInput_OnCloseChest(object sender, EventArgs e){
		if(selectedObject != null){
			if(selectedObject.TryGetComponent(out ChestObject chestObject)){
				chestObject.CloseChest();
			}
		}
	}

	private void GameInput_OnUseItem(object sender, EventArgs e){
		if(selectedHotbarSlot != -1 && playerInventoryObjectList[selectedHotbarSlot] != null){
			if(playerInventoryObjectList[selectedHotbarSlot].GetInventoryObjectSO() is GunItem gunItem){
				gunItem.Shoot();
			}
		}
	}

	private void GameInput_OnOpenInventory(object sender, GameInput.OnOpenInventoryEventArgs e){
		if(isInventoryOpen){
			isInventoryOpen = false;
			EventManager.Instance.OpenInventory(false);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			canMoveCamera = true;		
		}
		else {
			if(!e.playerClosedInventory){
				isInventoryOpen = true;
				EventManager.Instance.OpenInventory(true);

				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;

				canMoveCamera = false;
			}

		}
	}

    private void EventManager_OnAccessTerminal(object sender, EventManager.OnAccessTerminalEventArgs e){
		if(e.openTerminal){
			canMove = false;
			playerUsingComputer = true;
		}
		else {
			canMove = true;
			playerUsingComputer = false;
		}
    }

    private void EventManager_OnPickUpItem(object sender, EventManager.OnPickUpItemEventArgs e){
		if(e.itemGoesIntoHotbar){
			for(int i = 0; i < 5; i++){
				if(playerInventoryObjectList[i] == null){
					//this should be handled in the update loop, so it loops through all the children of hotbar and inventory and
					//sets the position to an inventory object IF the slot has a UI prefab as a child
					// playerInventoryObjectList[i] = e.inventoryObject;
					// numItemsInHotbar++;

					//fire an event that makes it so that it instantiates a thing at the current hotbar position
					//should call a method in EventManager that fires an event that takes in "i" as the hotbar position and the inventory object
					//then that should instantiate a prefab in the UI script for the hotbar that instantiates the item prefab and sets the sprite to the object sprite
					EventManager.Instance.AddItemToHotbar(i, e.inventoryObject);
					break;
				}
			}
		} else {
			for(int i = 0; i < 15; i++){
				if(playerMainInventoryObjectList[i] == null){
					EventManager.Instance.AddItemToInventory(i, e.inventoryObject);
					break;
				}
			}
		}
		
    }

    public Vector3 GetPlayerLookDirNormalized(){
		return playerCamera.transform.forward.normalized;
    }

    public Vector3 GetPlayerCameraPos(){
		return playerCamera.transform.position;
    }

	public int GetNumInventoryObjectsInHotbar(){
		return numItemsInHotbar;
	}

	public int GetNumInventoryObjectsOverall(){
		return numItemsInHotbar + numItemInInventory;
	}

	public bool IsPlayerUsingComputer(){
		return playerUsingComputer;
	}

}
