using UnityEngine;

public class forceApplier : MonoBehaviour  
{
    public float forceMagnitude = 10f;
    public float cooldownTime = 1f;
    private float lastUsedTime = 0f;

    void Start()
    {
        lastUsedTime = -cooldownTime;
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (Input.GetButton("Fire1") && lastUsedTime + cooldownTime < Time.time)
        {
            Rigidbody rb = collisionInfo.GetComponent<Collider>().GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = collisionInfo.transform.position - transform.position;
                forceDirection.y = 0; // Keep the force horizontal
                forceDirection.Normalize();
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                lastUsedTime = Time.time;
            }
        }
    }
}
