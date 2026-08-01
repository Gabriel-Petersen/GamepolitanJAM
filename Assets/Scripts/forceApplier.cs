using UnityEngine;

public class forceApplier : MonoBehaviour  
{
    public float forceMagnitudeX = 10f;
    public float forceMagnitudeY = 1f;
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
                Vector3 forceDirectionX = collisionInfo.transform.position - transform.position;
                forceDirectionX.y = 0; // Keep the force horizontal
                forceDirectionX.Normalize();


                rb.AddForce(forceDirectionX * forceMagnitudeX + Vector3.up * forceMagnitudeY, ForceMode.Impulse);
                lastUsedTime = Time.time;
            }
        }
    }
}
