using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    readonly int moveTreeHash = Animator.StringToHash("MoveTree");
    public EnemyMoveState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(moveTreeHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }
}
