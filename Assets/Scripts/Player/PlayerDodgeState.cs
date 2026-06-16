using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    readonly int dodgeTreeHash = Animator.StringToHash("DodgeTree");
    readonly int dodgeForwardHash = Animator.StringToHash("DodgeForward");
    readonly int dodgeSideHash = Animator.StringToHash("DodgeSide");

    float remainingDodgeTime = 0;
    Vector3 direction;

    public PlayerDodgeState(PlayerStateMachine stateMachine, Vector3 direction) : base(stateMachine)
    {
        this.direction = direction;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;

        stateMachine.AnimationController.SetFloat(dodgeSideHash, direction.y);
        stateMachine.AnimationController.SetFloat(dodgeForwardHash, direction.x);
        stateMachine.AnimationController.CrossFadeInFixedTime(dodgeTreeHash, 0.1f);
        stateMachine.Health.SetVulnerability(true);
    }

    public override void Tick(float deltaTime)
    {
        float sideMove = direction.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        float forwardMove = direction.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        Vector3 moveDirection = new Vector3(sideMove, 0, forwardMove);
        Vector3 targetDirection = Quaternion.AngleAxis(stateMachine.MainCamera.eulerAngles.y, Vector3.up) * moveDirection;

        Move(targetDirection, deltaTime);
        FaceTarget();
        remainingDodgeTime -= deltaTime;

        if (remainingDodgeTime <= 0f)
        {
            stateMachine.SwitchState(new PlayerTargetState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetVulnerability(false);
    }
}
