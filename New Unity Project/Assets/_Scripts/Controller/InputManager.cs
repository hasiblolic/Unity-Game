using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public CameraManager cameraManager;
	public Controller controller;
	public Transform camTransform;

	//Triggers & bumpers
	bool Rb, Rt, Lb, Lt, isAttacking, b_Input, y_Input, x_Input, inventoryInput,
	leftArrow, rightArrow, upArrow, downArrow;

	float vertical;
	float horizontal;
	float moveAmount;
	float mouseX;
	float mouseY;
	bool rollFlag;
	float rollTimer;

	public PlayerProfile playerProfile;

	public ExuctionOrder cameraMovement;

	public enum ExuctionOrder
	{
		fixedUpdate, update, lateUpdate
	}

	private void Start()
	{
		//TODO check if you have the controller assigned and if not, Instantiate it
		camTransform = Camera.main.transform;

		ResourcesManager rm = GameSettings.resourcesManager;
		for (int i = 0; i < playerProfile.startingClothes.Length; i++)
		{
			BaseItem item = rm.GetItem(playerProfile.startingClothes[i]);
			if (item is ClothItem)
			{
				controller.startingCloth.Add((ClothItem)item);
			}
		}

		controller.Init();
		controller.SetWeapons(rm.GetItem(playerProfile.rightHandWeapon), rm.GetItem(playerProfile.leftHandWeapon));
		cameraManager.targetTransform = controller.transform;
	}

	private void FixedUpdate()
	{
		if (controller == null)
			return;

		float delta = Time.fixedDeltaTime;

		HandleMovement(delta);
		cameraManager.FollowTarget(delta);

		if (cameraMovement == ExuctionOrder.fixedUpdate)
		{
			cameraManager.HandleRotation(delta, mouseX, mouseY);
		}
	}

	private void Update()
	{
		if (controller == null)
			return;

		float delta = Time.deltaTime;

		HandleInput();

		if (b_Input)
		{
			rollFlag = true;
			rollTimer += delta;
		}

		if (cameraMovement == ExuctionOrder.update)
		{
			cameraManager.HandleRotation(delta, mouseX, mouseY);
		}
	}

	private void LateUpdate()
	{
		if (cameraMovement == ExuctionOrder.lateUpdate)
		{
			//cameraManager.FollowTarget(Time.deltaTime);
		}
	}

	void HandleMovement(float delta)
	{
		Vector3 movementDirection = camTransform.right * horizontal;
		movementDirection += camTransform.forward * vertical;
		movementDirection.Normalize();

		controller.MoveCharacter(vertical, horizontal, movementDirection, delta);
	}

	void HandleInput()
	{
		bool retVal = false;
		isAttacking = false;

		vertical = Input.GetAxis("Vertical");
		horizontal = Input.GetAxis("Horizontal");
		moveAmount = Mathf.Clamp01(Mathf.Abs(vertical) + Mathf.Abs(horizontal));

		Rb = Input.GetButton("RB");
		Rt = Input.GetButton("RT");
		Lb = Input.GetButton("LB");
		Lt = Input.GetButton("LT");
		inventoryInput = Input.GetButton("Inventory");
		b_Input = Input.GetButton("B");
		y_Input = Input.GetButtonDown("Y");
		x_Input = Input.GetButton("X");
		leftArrow = Input.GetButton("Left");
		rightArrow = Input.GetButton("Right");
		upArrow = Input.GetButton("Up");
		downArrow = Input.GetButton("Down");
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");



		if (!controller.isInteracting)
		{
			if (retVal == false)
				retVal = HandleRolls();
		}

		if (retVal == false)
			retVal = HandleAttacking();


		if (Input.GetKeyDown(KeyCode.F))
		{
			if (controller.lockOn)
			{
				cameraManager.lockTarget = null;
				controller.lockOn = false;
				controller.currentLockTarget = null;

			}
			else
			{
				Transform lockTarget = controller.FindLocakbleTarget();

				if (lockTarget != null)
				{
					cameraManager.lockTarget = lockTarget;
					controller.lockOn = true;
					controller.currentLockTarget = lockTarget;
				}
				else
				{
					cameraManager.lockTarget = null;
					controller.lockOn = false;
				}
			}
		}


	}

	bool HandleAttacking()
	{
		AttackInputs attackInput = AttackInputs.none;

		if (Rb || Rt || Lb || Lt)
		{
			isAttacking = true;

			if (Rb)
			{
				attackInput = AttackInputs.rb;
			}

			if (Rt)
			{
				attackInput = AttackInputs.rt;
			}
			if (Lb)
			{
				attackInput = AttackInputs.lb;
			}
			if (Lt)
			{
				attackInput = AttackInputs.lt;
			}
		}

		if (y_Input)
		{

		}
		if (attackInput != AttackInputs.none)
		{
			if (!controller.isInteracting)
			{
				controller.PlayTargetItemAction(attackInput);
			}
			else
			{
				if (controller.canDoCombo)
				{
					controller.DoCombo(attackInput);
				}
			}
		}

		return isAttacking;
	}

	bool HandleRolls()
	{
		if (b_Input == false && rollFlag)
		{
			rollFlag = false;

			if (moveAmount > 0)//rollTimer > 0.5f ||
			{
				Vector3 movementDirection = camTransform.right * horizontal;
				movementDirection += camTransform.forward * vertical;
				movementDirection.Normalize();
				movementDirection.y = 0;

				Quaternion dir = Quaternion.LookRotation(movementDirection);
				controller.transform.rotation = dir;
				controller.PlayTargetAnimation("Roll", true, false, 1.5f);
				return true;
			}
			else
			{
				controller.PlayTargetAnimation("Step Back", true, false);
			}
		}

		return false;
	}

}