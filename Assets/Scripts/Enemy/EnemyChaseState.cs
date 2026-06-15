using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    readonly int moveTreeHash = Animator.StringToHash("MoveTree");
    readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");

    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(moveTreeHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyMoveState(stateMachine));
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
            return;
        }

        MoveToPlayer(Time.deltaTime);
        FacePlayer();
        stateMachine.Animator.SetFloat(moveSpeedHash, 1f, 0.1f, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    void MoveToPlayer(float deltaTime)
    {
        stateMachine.Agent.SetDestination(stateMachine.Player.transform.position);

        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    bool IsInAttackRange()
    {
        float sqrDistance = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return sqrDistance <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
