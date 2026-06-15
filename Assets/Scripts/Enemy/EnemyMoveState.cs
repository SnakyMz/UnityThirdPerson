using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    readonly int moveTreeHash = Animator.StringToHash("MoveTree");
    readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    public EnemyMoveState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(moveTreeHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        Move(Time.deltaTime);

        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChaseState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(moveSpeedHash, 0f, 0.1f, deltaTime);
    }

    public override void Exit()
    {

    }
}
