using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    readonly int moveTreeHash = Animator.StringToHash("MoveTree");
    public CharacterController Controller { get; private set; }
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
            // Enter Chase State
            return;
        }
    }

    public override void Exit()
    {

    }
}
