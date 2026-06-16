using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    readonly int TargetTreeHash = Animator.StringToHash("TargetTree");
    readonly int TargetForwardHash = Animator.StringToHash("TargetForward");
    readonly int TargetSideHash = Animator.StringToHash("TargetSide");

    public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (!stateMachine.Targeter.SelectTarget())
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }
        stateMachine.AnimationController.CrossFadeInFixedTime(TargetTreeHash, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
            return;
        }
        else if (stateMachine.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockState(stateMachine));
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }

        float forwardMove = 0;
        float sideMove = 0;
        Vector3 moveDirection = Vector3.zero;

        if (stateMachine.remainingDodgeTime > 0f)
        {
            sideMove = stateMachine.MoveInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
            forwardMove = stateMachine.MoveInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;
            moveDirection = new Vector3(sideMove, 0, forwardMove);

            stateMachine.remainingDodgeTime = Mathf.Max(stateMachine.remainingDodgeTime - deltaTime, 0f);
        }
        else
        {
            sideMove = stateMachine.MoveInput.x;
            forwardMove = stateMachine.MoveInput.y;
            moveDirection = new Vector3(sideMove, 0, forwardMove).normalized;
        }
        Vector3 targetDirection = Quaternion.AngleAxis(stateMachine.MainCamera.eulerAngles.y, Vector3.up) * moveDirection;

        stateMachine.AnimationController.SetFloat(TargetForwardHash, forwardMove);
        stateMachine.AnimationController.SetFloat(TargetSideHash, sideMove);
        Move(targetDirection * stateMachine.TargetSpeed, deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {

    }
}
