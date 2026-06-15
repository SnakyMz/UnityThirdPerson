using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] public int attackDamage = 20;
    [SerializeField] public int knockback = 20;
    [SerializeField] public GameObject weaponHitbox;
    [field: SerializeField] public float DetectionRange { get; private set; } = 10f;
    [field: SerializeField] public float MovementSpeed { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    public Animator Animator { get; private set; }
    public GameObject Player { get; private set; }
    public CharacterController Controller { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Vector3 Velocity { get; private set; }
    public Weapon Weapon { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Ragdoll Ragdoll { get; private set; }

    protected EnemyBaseState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Weapon = weaponHitbox.GetComponent<Weapon>();
        Agent = GetComponent<NavMeshAgent>();
        Controller = GetComponent<CharacterController>();
        Player = FindFirstObjectByType<PlayerStateMachine>().gameObject;
        ForceReceiver = GetComponent<ForceReceiver>();
        Animator = GetComponent<Animator>();
        Ragdoll = GetComponent<Ragdoll>();
        SwitchState(new EnemyMoveState(this));

        Agent.updatePosition = false;
        Agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Tick(Time.deltaTime);

        Velocity = ForceReceiver.AddGravity();
    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
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
        SwitchState(new EnemyImpactState(this));
    }

    void HandleDeath()
    {
        SwitchState(new EnemyDeathState(this));
    }
}
