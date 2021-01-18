using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboBehaviour : StateMachineBehaviour
{
	public Combo[] combos;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Controller controller = animator.GetComponentInParent<Controller>();
		controller.LoadCombos(combos);
	}


}
