using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    readonly int TargetTreeHash = Animator.StringToHash("TargetTree");
    public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (!stateMachine.Targeter.SelectTarget())
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }
        else
        {
            stateMachine.AnimationController.Play(TargetTreeHash);
            stateMachine.TargetGroup.AddMember(stateMachine.Targeter.CurrentTarget.transform, 1, 2);
        }
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }
}
