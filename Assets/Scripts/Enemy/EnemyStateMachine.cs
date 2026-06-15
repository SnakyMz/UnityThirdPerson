using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.AI;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] float drag = 0.4f;
    [field: SerializeField] public float DetectionRange { get; private set; } = 10f;
    [field: SerializeField] public float MovementSpeed { get; private set; } = 4f;
    public Animator Animator { get; private set; }
    public GameObject Player { get; private set; }
    public CharacterController Controller { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Vector3 Velocity { get; private set; }
    float verticalVelocity;
    Vector3 dampingVelocity;
    Vector3 impact;

    protected EnemyBaseState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
}
