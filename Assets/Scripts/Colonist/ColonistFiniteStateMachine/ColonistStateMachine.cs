using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonistStateMachine
{
    public ColonistState CurrentState { get; private set; }

    public void Initialize(ColonistState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void ChangeState(ColonistState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
