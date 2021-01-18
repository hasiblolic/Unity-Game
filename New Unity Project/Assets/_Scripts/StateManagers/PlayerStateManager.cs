using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStateManager : CharacterStateManager
{
	[Header("Inputs")]
	public float mouseX;
	public float mouseY;
	public float moveAmount;
	public Vector3 rotateDirection;
	public Vector3 rollDirection;

	[Header("References")]
	public new Transform camera;
	public Cinemachine.CinemachineFreeLook normalCamera;
	public Cinemachine.CinemachineFreeLook lockOnCamera;


	[Header("Movement Stats")]
	public float frontRayOffset = .5f;
	public float movementSpeed = 1;
	public float rollSpeed = 1;
	public float adaptSpeed = 1;
	public float rotationSpeed = 10;
	public float attackRotationSpeed = 3;
	public float navMeshDetectDistance = 1;
	public bool isRolling;
	public AnimationCurve rollCurve;
	public AnimationClip rollClip;

	[HideInInspector]
	public LayerMask ignoreForGroundCheck;

	public string locomotionId = "locomotion";
	public string attackStateId = "attackId";
	public string rollStateId = "waitState";



	public override void Init()
	{
		base.Init();

		MovePlayerCharacter movePlayerCharacter = new MovePlayerCharacter(this);
		InterruptAnimationIfMoveInput interruptAnimation = new InterruptAnimationIfMoveInput(this, locomotionId);

		State locomotion = new State(
			new List<StateAction>() //Fixed Update
			{
					movePlayerCharacter
			},
			new List<StateAction>() //Update
			{

			},
			new List<StateAction>()//Late Update
			{

			}
		);

		locomotion.onEnter = DisableRootMotion;
		//	locomotion.onEnter += DisableCombo;

		MonitorInteractingAnimation monitorInteractingAnimation = new MonitorInteractingAnimation(this, "isInteracting", locomotionId);

		State attackState = new State(
			new List<StateAction>() //Fixed Update
			{
					new HandleRotationHook(this,movePlayerCharacter)
			},
			new List<StateAction>() //Update
			{
					monitorInteractingAnimation,
					new InputsForCombo(this),
					interruptAnimation

			},
			new List<StateAction>()//Late Update
			{

			}
		);

		attackState.onEnter = EnableRootMotion;
		attackState.onEnter += DisableComboVariables;

		State rollState = new State(
			new List<StateAction>() //Fixed Update
			{
					new HandleRollVelocity(this)
			},
			new List<StateAction>() //Update
			{
					monitorInteractingAnimation
				//		new InputsForCombo(this)

			},
			new List<StateAction>()//Late Update
				{
			}
			);

		RegisterState(locomotionId, locomotion);
		RegisterState(attackStateId, attackState);
		RegisterState(rollStateId, rollState);

		ChangeState(locomotionId);

		ignoreForGroundCheck = ~(1 << 9 | 1 << 10);

		clothManager.Init();
		LoadListOfItems(startingsCloths);

		weaponHolderManager.Init();
		weaponHolderManager.LoadWeaponOnHook(leftWeapon, true);
		weaponHolderManager.LoadWeaponOnHook(rightWeapon, false);
		UpdateItemActionsWithCurrent();
	}

	void LoadListOfItems(List<ClothItem> targetClothes)
	{
		clothManager.LoadListOfItems(targetClothes);
	}

	private void FixedUpdate()
	{
		delta = Time.fixedDeltaTime;

		base.FixedTick();
	}

	public bool debugLock;

	private void Update()
	{
		delta = Time.deltaTime;
		base.Tick();
	}

	private void LateUpdate()
	{
		base.LateTick();
	}

	#region Lock on 
	public override void OnAssignLookOverride(Transform target)
	{
		base.OnAssignLookOverride(target);
		if (lockOn == false)
			return;

		normalCamera.gameObject.SetActive(false);
		lockOnCamera.gameObject.SetActive(true);
		lockOnCamera.m_LookAt = target;
	}

	public override void OnClearLookOverride()
	{
		base.OnClearLookOverride();
		normalCamera.gameObject.SetActive(true);
		lockOnCamera.gameObject.SetActive(false);
	}

	#endregion

	

	public override void DoCombo(AttackInputs inp)
	{
		//	Debug.Log("DoCombo");
		Combo c = GetComboFromInp(inp);

		if (c == null)
			return;

		AssignCurrentWeaponAndAction(currentWeaponInUse, currentItemAction);
		PlayTargetAnimation(c.animName, true, currentItemAction.isMirrored);
		//currentItemAction.ExecuteItemAction(this);
		ChangeState(attackStateId);
		canDoCombo = false;
	}

	Combo GetComboFromInp(AttackInputs inp)
	{
		if (currentCombo == null)
			return null;

		for (int i = 0; i < currentCombo.Length; i++)
		{
			if (currentCombo[i].inp == inp)
				return currentCombo[i];
		}

		return null;
	}

	#region State Events
	void DisableRootMotion()
	{
		useRootMotion = false;
	}

	void EnableRootMotion()
	{
		useRootMotion = true;
	}

	void DisableComboVariables()
	{
		//	canDoCombo = false;		
	}
	#endregion

}