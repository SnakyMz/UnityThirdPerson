using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHangState : PlayerBaseState
{
    readonly int hangHash = Animator.StringToHash("Hanging");

    Vector3 closestPoint;
    Vector3 ledgeForward;

    public PlayerHangState(PlayerStateMachine stateMachine, Vector3 closestPoint, Vector3 ledgeForward) : base(stateMachine)
    {
        this.closestPoint = closestPoint;
        this.ledgeForward = ledgeForward;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        stateMachine.AnimationController.CrossFadeInFixedTime(hangHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.MoveInput.y < 0f)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }
    }

    public override void Exit()
    {

    }
}
