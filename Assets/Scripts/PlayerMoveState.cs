using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    readonly int MoveTreeHash = Animator.StringToHash("MoveTree");
    readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.AnimationController.Play(MoveTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movedirection = new Vector3(stateMachine.MoveInput.x, 0, stateMachine.MoveInput.y).normalized;
        Vector3 cameraDirection = Quaternion.AngleAxis(stateMachine.MainCamera.eulerAngles.y, Vector3.up) * movedirection;

        stateMachine.AnimationController.SetFloat(MoveSpeedHash, movedirection.magnitude);
        stateMachine.Controller.Move(cameraDirection * stateMachine.MoveSpeed * deltaTime);

        if (stateMachine.MoveInput == Vector2.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(cameraDirection);
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, targetRotation, deltaTime * stateMachine.TurnSpeed);
    }

    public override void Exit()
    {

    }
}
