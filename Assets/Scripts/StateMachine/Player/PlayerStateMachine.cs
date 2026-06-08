using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }

    PlayerInput playerInput;

    protected PlayerBaseState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;

        SwitchState(new PlayerIdleState(this));
    }

    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            MoveInput = context.ReadValue<Vector2>();
        }
    }
    public void SwitchState(PlayerBaseState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
