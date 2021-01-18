using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    bool forceExit;
    List<StateAction> fixedUpdateActions;
    List<StateAction> updateActions;
    List<StateAction> lateUpdateActions;

    public delegate void OnEnter();
    public OnEnter onEnter;


    public State(List<StateAction> fixedUpdateActions, List<StateAction> updateActions, List<StateAction> lateUpdateActions)
    {
        this.fixedUpdateActions = fixedUpdateActions;
        this.updateActions = updateActions;
        this.lateUpdateActions = lateUpdateActions;
    }

    public void FixedTick()
    {
        ExecuteListOfActions(fixedUpdateActions);
    }

    public void Tick()
    {
        ExecuteListOfActions(updateActions);
    }

    public void LateTick()
    {
        ExecuteListOfActions(lateUpdateActions);
        forceExit = false;
    }

    void ExecuteListOfActions(List<StateAction> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (forceExit) return;

            forceExit = actions[i].Execute();
        }
    }
}
