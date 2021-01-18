using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHook : MonoBehaviour
{
	Controller controller;
	Animator animator;

	// AI
	public bool hasLookAtTarget;
	public Vector3 lookAtPosition;
	bool isAI;

	public bool canRotate;
	public bool canDoCombo;
	public Vector3 deltaPosition;

	private void Start()
	{
		animator = GetComponent<Animator>();
		controller = GetComponentInParent<Controller>();

		if(controller == null)
		{
			isAI = true;
		}
		else
		{
			isAI = false;
		}
	}

	public void OnAnimatorMove()
	{
		OnAnimatorMoveOverrride();
	}

	protected virtual void OnAnimatorMoveOverrride()
	{
		float delta = Time.deltaTime;
		if (!isAI)
		{
			if (controller.isInteracting == false)
				return;

			if (controller.isGrounded && delta > 0)
				deltaPosition = (animator.deltaPosition) / delta;
		}
		else
		{
			deltaPosition = (animator.deltaPosition) / delta;
		}
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (hasLookAtTarget)
		{
			animator.SetLookAtWeight(1, 0.9f, .95f, 1, 1);
			animator.SetLookAtPosition(lookAtPosition);
		}
	}

	public void OpenCanMove()
	{
		controller.canMove = true;
	}

	public void OpenDamageCollider()
	{
		//		controller.HandleDamageCollider(true);
	}

	public void CloseDamageCollider()
	{
		//	controller.HandleDamageCollider(false);
	}

	public void EnableCombo()
	{
		canDoCombo = true;
	}

	public void EnableRotation()
	{
		canRotate = true;
	}

	public void DisableRotation()
	{
		canRotate = false;
	}


}