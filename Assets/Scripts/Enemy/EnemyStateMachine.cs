using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] float drag = 0.4f;
    [SerializeField] public int attackDamage = 10;
    [SerializeField] public GameObject weaponHitbox;
    [field: SerializeField] public float DetectionRange { get; private set; } = 10f;
    [field: SerializeField] public float MovementSpeed { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackDamage { get; private set; } = 10f;
    public Animator Animator { get; private set; }
    public GameObject Player { get; private set; }
    public CharacterController Controller { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Vector3 Velocity { get; private set; }
    public Weapon Weapon { get; private set; }

    float verticalVelocity;
    Vector3 dampingVelocity;
    Vector3 impact;

    protected EnemyBaseState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Weapon = weaponHitbox.GetComponent<Weapon>();
        Agent = GetComponent<NavMeshAgent>();
        Controller = GetComponent<CharacterController>();
        Player = FindFirstObjectByType<PlayerStateMachine>().gameObject;
        Animator = GetComponent<Animator>();
        SwitchState(new EnemyMoveState(this));

        Agent.updatePosition = false;
        Agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Tick(Time.deltaTime);

        AddGravity();
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

    public void EnableWeaponHitbox()
    {
        weaponHitbox.SetActive(true);
    }

    public void DisableWeaponHitbox()
    {
        weaponHitbox.SetActive(false);
    }
}
