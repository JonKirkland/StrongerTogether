using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STStateManager : MonoBehaviour
{
    public State currentState;
    void Update()
    {
        RunStateMachine();
    }
    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState(); //? means if var not null, run current state, else ignore

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }
    private void SwitchToNextState(State nextState)
    {
        //switch current state, to state passed in
        currentState = nextState;
    }
}
