using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager : MonoBehaviour
{
    State currentState;
    Dictionary<string, State> allStates = new Dictionary<string, State>();

    [HideInInspector]
    public Transform mTransform;

    private void Start()
    {
        mTransform = this.transform;
        Init();
    }

    public abstract void Init();

    public void FixedTick()
    {
        if (currentState == null) return;

        currentState.FixedTick();
    }

    public void Tick()
    {
        if (currentState == null) return;

        currentState.Tick();
    }

    public void LateTick()
    {
        if (currentState == null) return;

        currentState.LateTick();
    }

    public void ChangeState(string targetID)
    {
        if (currentState != null)
        {
            // run on exit actions of currentstate
        }

        State targetState = GetState(targetID);
        // run on enter actions

        currentState = targetState;

        currentState.onEnter?.Invoke();
    }

    State GetState(string targetID)
    {
        allStates.TryGetValue(targetID, out State retValue);
        return retValue;
    }

    protected void RegisterState(string stateID, State state)
    {
        allStates.Add(stateID, state);
    }
}
