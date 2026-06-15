using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    readonly int impactHash = Animator.StringToHash("Impact");

    float duration = 1f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(impactHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyMoveState(stateMachine));
        }
    }

    public override void Exit()
    {

    }
}
