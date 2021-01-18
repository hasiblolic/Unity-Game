using UnityEngine;
using System.Collections;

public class HandleRotationHook : StateAction
{
    PlayerStateManager states;
    MovePlayerCharacter move;

    public HandleRotationHook(PlayerStateManager psm, MovePlayerCharacter mpc)
    {
        this.states = psm;
        this.move = mpc;
    }

    public override bool Execute()
    {
        if (states.canRotate)
        {
            move.HandleRotation();
        }

        return false;
    }
}
