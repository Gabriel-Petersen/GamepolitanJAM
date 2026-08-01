using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float acceleration = 20f;
    private Rigidbody rb;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindFirstObjectByType<PlayerController>().gameObject;
  
    }

    /*
    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = player.transform.position - transform.position;

    }
    */

    void FixedUpdate()
    {
        if (player == null) return;

        // 1. Calculate direction to the player/target
        Vector3 targetDirection = (player.transform.position - transform.position).normalized;
        //targetDirection.y = 0; // Keep movement on the flat plane

        // 2. Check if an external force (knockback) made the enemy exceed its normal speed
        // If the enemy is moving too fast, let the knockback physics take over naturally
        if (rb.linearVelocity.magnitude > moveSpeed + 1f)
        {
            return;
        }

        // 3. Calculate how much velocity we need to add to match our desired movement
        Vector3 targetVelocity = targetDirection * moveSpeed;
        Vector3 velocityError = targetVelocity - new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // 4. Gently or firmly force the enemy toward the target velocity without breaking physics
        Vector3 movementForce = velocityError * acceleration * Time.fixedDeltaTime;
        rb.AddForce(movementForce, ForceMode.VelocityChange);
    }

}



   