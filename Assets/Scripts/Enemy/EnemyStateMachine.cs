using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public Animator Animator { get; private set; }

    protected EnemyBaseState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator = GetComponent<Animator>();
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
