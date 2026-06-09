using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.Velocity) * deltaTime);
    }

    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget == null) return;

        Vector3 targetDirection = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        targetDirection.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, targetRotation, stateMachine.TurnSpeed * Time.deltaTime);
    }
}
