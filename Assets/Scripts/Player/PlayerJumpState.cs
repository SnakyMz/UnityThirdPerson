using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    readonly int jumpHash = Animator.StringToHash("Jump");

    Vector3 momentum = Vector3.zero;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ForceReceiver.AddJump(stateMachine.JumpForce);
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;
        stateMachine.AnimationController.CrossFadeInFixedTime(jumpHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {

    }
}
