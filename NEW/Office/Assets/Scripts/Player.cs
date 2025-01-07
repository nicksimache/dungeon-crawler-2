using UnityEngine;
using System;
using System.Collections.Generic;


public class Player : MonoBehaviour {

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

	private bool canMove = true;

    public void Start() {
		gameInput.OnInteractAction += GameInput_OnInteractAction;
		gameInput.OnStopInteract += GameInput_OnStopInteract;
		gameInput.OnCloseTerminal += GameInput_OnCloseTerminal;

		tempObj.OnAccessTerminal += ComputerObject_OnAccessTerminal;

		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

	void Update()
    {
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

    private void GameInput_OnInteractAction (object sender, EventArgs e){
		tempObj.Interact(this);
    }

    private void GameInput_OnStopInteract (object sender, EventArgs e){
		tempObj.ResetProgressBar();
    }

    private void GameInput_OnCloseTerminal(object sender, EventArgs e){
		tempObj.CloseTerminal();
    }

	private void ComputerObject_OnAccessTerminal(object sender, EventArgs e){
		if(e.openTerminal){
			canMove = false;
		}
		else {
			canMove = true;
		}
	}

}
