using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    public Vector2 MoveInput { get; private set; }

    PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;

        SwitchState(new PlayerIdleState(this));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name == "Move")
        {
            MoveInput = context.ReadValue<Vector2>();
        }
    }
}
