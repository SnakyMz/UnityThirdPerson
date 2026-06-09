using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    readonly int TargetTreeHash = Animator.StringToHash("TargetTree");
    public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.AnimationController.Play(TargetTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.Targeter.SelectTarget())
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }

        Vector3 moveDirection = new Vector3(stateMachine.MoveInput.x, 0, stateMachine.MoveInput.y).normalized;
        Vector3 targetDirection = Quaternion.AngleAxis(stateMachine.MainCamera.eulerAngles.y, Vector3.up) * moveDirection;
        Move(targetDirection * stateMachine.TargetSpeed, deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {

    }
}
