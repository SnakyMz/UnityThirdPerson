using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    readonly int blockHash = Animator.StringToHash("Block");

    public PlayerBlockState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Health.SetVulnerability(true);
        stateMachine.AnimationController.CrossFadeInFixedTime(blockHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (!stateMachine.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetVulnerability(false);
    }
}
