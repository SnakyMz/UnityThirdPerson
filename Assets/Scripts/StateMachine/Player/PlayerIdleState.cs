using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3(stateMachine.MoveInput.x, 0, stateMachine.MoveInput.y);
        stateMachine.transform.Translate(movement * deltaTime * 5f);
    }

    public override void Exit()
    {

    }
}
