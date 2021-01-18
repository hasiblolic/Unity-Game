using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptAnimationIfMoveInput : StateAction
{
	PlayerStateManager states;
	string locomotionId;

	public InterruptAnimationIfMoveInput(PlayerStateManager s, string locomotion)
	{
		states = s;
		locomotionId = locomotion;
	}

	public override bool Execute()
	{
		if (states.canMove)
		{
			if (states.horizontal != 0 || states.vertical != 0)
			{
				states.anim.Play("Empty");
				states.ChangeState(locomotionId);
				states.canMove = false;
				return true;
			}
			else
			{
				states.canMove = false;
				return false;
			}
		}

		return false;
	}
}
