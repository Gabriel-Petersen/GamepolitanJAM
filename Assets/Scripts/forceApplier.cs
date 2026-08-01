using UnityEngine;

public class forceApplier : MonoBehaviour  
{
    public float forceMagnitude = 10f;
 
    

    void OnCollisionStay(Collision collisionInfo)
    {
        if (Input.GetButton("Attack"))
        {
            Rigidbody rb = collisionInfo.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = collisionInfo.transform.position - transform.position;
                forceDirection.y = 0; // Keep the force horizontal
                forceDirection.Normalize();
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
            }
        }
    }
}
