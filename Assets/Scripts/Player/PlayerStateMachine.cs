using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] public GameObject weaponHitbox;
    public Vector3 Velocity { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator AnimationController { get; private set; }
    public Transform MainCamera { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsBlocking { get; private set; }
    public CinemachineTargetGroup TargetGroup { get; private set; }
    public Targeter Targeter { get; private set; }
    public Weapon Weapon { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float TargetSpeed { get; private set; }
    [field: SerializeField] public float TurnSpeed { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    protected PlayerBaseState currentState;

    PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Weapon = weaponHitbox.GetComponent<Weapon>();
        Controller = GetComponent<CharacterController>();
        AnimationController = GetComponent<Animator>();
        MainCamera = Camera.main.transform;
        TargetGroup = GetComponentInChildren<CinemachineTargetGroup>();
        Targeter = GetComponentInChildren<Targeter>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Ragdoll = GetComponent<Ragdoll>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;

        SwitchState(new PlayerMoveState(this));
    }

    void Update()
    {
        currentState?.Tick(Time.deltaTime);

        Velocity = ForceReceiver.AddGravity();
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

        if (context.action.name == "Block")
        {
            if (context.performed)
            {
                IsBlocking = true;
            }
            else if (context.canceled)
            {
                IsBlocking = false;
            }
        }

    }
    public void SwitchState(PlayerBaseState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void EnableWeaponHitbox()
    {
        weaponHitbox.SetActive(true);
    }

    public void DisableWeaponHitbox()
    {
        weaponHitbox.SetActive(false);
    }

    void OnEnable()
    {
        Health.OnDamage += HandleDamage;
        Health.OnDie += HandleDeath;
    }

    void OnDisable()
    {
        Health.OnDamage -= HandleDamage;
        Health.OnDie -= HandleDeath;
    }

    void HandleDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    void HandleDeath()
    {
        SwitchState(new PlayerDeathState(this));
    }
}
