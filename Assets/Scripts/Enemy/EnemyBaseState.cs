using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    protected void Move(float deltaTime)
    {
        stateMachine.Controller.Move(stateMachine.Velocity * deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.Velocity) * deltaTime);
    }

    protected bool IsInChaseRange()
    {
        Vector3 distance = stateMachine.Player.transform.position - stateMachine.transform.position;
        return distance.magnitude <= stateMachine.detectionRange;
    }
}
