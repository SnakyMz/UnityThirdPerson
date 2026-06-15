using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    readonly int AttackHash = Animator.StringToHash("Attack");
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttackDamage(stateMachine.attackDamage);
        stateMachine.Animator.Play(AttackHash);
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }
}
