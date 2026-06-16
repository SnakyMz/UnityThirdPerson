using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] float drag = 0.4f;
    NavMeshAgent agent;

    CharacterController controller;
    float verticalVelocity;
    Vector3 impact = Vector3.zero;
    Vector3 dampingVelocity;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        agent = TryGetComponent<NavMeshAgent>(out NavMeshAgent component) ? component : null;
    }

    public Vector3 AddGravity()
    {
        if (verticalVelocity < 0 && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null && impact.sqrMagnitude <= 0.2f * 0.2f)
        {
            impact = Vector3.zero;
            agent.enabled = true;
        }

        return (Vector3.up * verticalVelocity) + impact;
    }

    public void AddImpact(Vector3 force)
    {
        impact += force;

        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    public void AddJump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }
}
