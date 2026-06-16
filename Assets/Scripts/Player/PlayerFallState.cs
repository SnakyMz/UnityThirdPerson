using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    readonly int fallHash = Animator.StringToHash("Fall");

    Vector3 momentum = Vector3.zero;

    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;
        stateMachine.AnimationController.Play(fallHash);

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedge;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedge;
    }

    void HandleLedge(Vector3 closestPoint, Vector3 ledgeForward)
    {
        stateMachine.SwitchState(new PlayerHangState(stateMachine, closestPoint, ledgeForward));
    }
}
