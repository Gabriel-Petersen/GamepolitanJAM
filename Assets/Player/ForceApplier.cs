using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForceApplier : MonoBehaviour  
{
    private Collider triggerCollider;
    [Tooltip("The force vector to apply to the Rigidbody. X is the horizontal force, Y is the vertical force.")]
    [SerializeField] private Vector2 forceVector;

    public bool Enabled
    {
        get
        {
            return triggerCollider != null && triggerCollider.enabled;
        }
        set
        {
            if (triggerCollider != null)
            {
                triggerCollider.enabled = value;
            }
        }
    }

    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 forceDirection = collisionInfo.transform.position - transform.position;
            forceDirection.y = 0; // Keep the force horizontal
            forceDirection.Normalize();
            rb.AddForce(forceDirection * forceVector.x + Vector3.up * forceVector.y, ForceMode.Impulse);
        }
    }
}
