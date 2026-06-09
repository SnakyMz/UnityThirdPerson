using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerStateMachine : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator AnimationController { get; private set; }
    public Transform MainCamera { get; private set; }
    public CinemachineTargetGroup TargetGroup { get; private set; }
    public Targeter Targeter { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float TurnSpeed { get; private set; }

    protected PlayerBaseState currentState;

    PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        AnimationController = GetComponent<Animator>();
        MainCamera = Camera.main.transform;
        TargetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        Targeter = GetComponentInChildren<Targeter>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;

        SwitchState(new PlayerMoveState(this));
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

        if (context.action.name == "Target" && context.performed)
        {
            SwitchState(new PlayerTargetState(this));
        }
    }
    public void SwitchState(PlayerBaseState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
