using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    State currentState;
    void Update()
    {
        if (currentState != null) currentState.Tick(Time.deltaTime);
    }

    public void SwitchState(State newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        if (currentState != null) currentState.Enter();
    }
}
