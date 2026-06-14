using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    protected EnemyBaseState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
