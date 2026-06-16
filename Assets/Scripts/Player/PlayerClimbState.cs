using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
    readonly int climbHash = Animator.StringToHash("Climbing");
    readonly Vector3 offset = new Vector3(0, 2.325f, 0.65f);

    public PlayerClimbState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.AnimationController.CrossFadeInFixedTime(climbHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.AnimationController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) return;

        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(offset, Space.Self);
        stateMachine.Controller.enabled = true;

        stateMachine.SwitchState(new PlayerMoveState(stateMachine, false));
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}
