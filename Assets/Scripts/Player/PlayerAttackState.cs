using UnityEditorInternal;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    Attack attack;

    bool forceApplied = false;

    public PlayerAttackState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttackDamage(attack.Damage);
        stateMachine.AnimationController.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime();
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
                stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            else
                stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }
    }

    public override void Exit()
    {

    }

    void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) return;

        if (normalizedTime < attack.ComboAttackTime) return;

        stateMachine.SwitchState(new PlayerAttackState(stateMachine, attack.ComboStateIndex));
    }

    void TryApplyForce()
    {
        if (forceApplied) return;

        stateMachine.AddImpact(stateMachine.transform.forward * attack.Force);

        forceApplied = true;
    }

    float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.AnimationController.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.AnimationController.GetNextAnimatorStateInfo(0);

        if (stateMachine.AnimationController.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!stateMachine.AnimationController.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
