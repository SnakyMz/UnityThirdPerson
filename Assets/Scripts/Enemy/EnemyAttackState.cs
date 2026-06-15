using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    readonly int AttackHash = Animator.StringToHash("Attack");
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(stateMachine.attackDamage, stateMachine.knockback);
        stateMachine.Animator.Play(AttackHash);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime() > 1f)
        {
            stateMachine.SwitchState(new EnemyMoveState(stateMachine));
        }
    }

    public override void Exit()
    {

    }

    float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
