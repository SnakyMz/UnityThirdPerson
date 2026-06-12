using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] float drag = 0.4f;
    public Vector3 Velocity { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator AnimationController { get; private set; }
    public Transform MainCamera { get; private set; }
    public bool IsAttacking { get; private set; }
    public CinemachineTargetGroup TargetGroup { get; private set; }
    public Targeter Targeter { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float TargetSpeed { get; private set; }
    [field: SerializeField] public float TurnSpeed { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    protected PlayerBaseState currentState;

    PlayerInput playerInput;

    float verticalVelocity;
    Vector3 impact = Vector3.zero;
    Vector3 dampingVelocity;


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

        AddGravity();
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

        if (context.action.name == "Cancel" && context.canceled)
        {
            SwitchState(new PlayerMoveState(this));
        }

        if (context.action.name == "Attack")
        {
            if (context.performed)
            {
                IsAttacking = true;
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
        }

    }
    public void SwitchState(PlayerBaseState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void AddGravity()
    {
        if (verticalVelocity < 0 && Controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
        Velocity = (Vector3.up * verticalVelocity) + impact;
    }

    public void AddImpact(Vector3 force)
    {
        impact += force;
    }
}
