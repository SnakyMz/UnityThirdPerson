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
        stateMachine.AnimationController.Play(TargetTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }

        float forwardMove = stateMachine.MoveInput.y;
        float sideMove = stateMachine.MoveInput.x;

        Vector3 moveDirection = new Vector3(sideMove, 0, forwardMove).normalized;
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
