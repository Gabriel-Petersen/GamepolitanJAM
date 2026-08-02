using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAi : MonoBehaviour, ISongResponsive
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float minimumPlayerDistance = 1f;

    private Rigidbody rb;
    private Transform playerTransform;
    private float currentSpeed;

    private bool isBeingPushed;
    private float knockbackTimer;

    public bool IsBeingPushed => isBeingPushed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        var playerObj = FindFirstObjectByType<PlayerController>();
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (playerTransform == null) return;

        if (isBeingPushed)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                isBeingPushed = false;
            }
            return;
        }

        if (Vector3.Distance(playerTransform.position, transform.position) < minimumPlayerDistance)
        {
            return;
        }

        Vector3 targetDirection = (playerTransform.position - transform.position).normalized;
        targetDirection.y = 0f;

        Vector3 targetVelocity = targetDirection * currentSpeed;
        Vector3 velocityError = targetVelocity - new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 movementForce = velocityError * acceleration * Time.fixedDeltaTime;
        rb.AddForce(movementForce, ForceMode.VelocityChange);

        currentSpeed = moveSpeed;
    }

    public void TriggerKnockback(Vector3 direction, Vector2 force, float duration)
    {
        isBeingPushed = true;
        knockbackTimer = duration;

        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

        Vector3 finalImpulse = (direction * force.x) + (Vector3.up * force.y);

        rb.AddForce(finalImpulse, ForceMode.Impulse);
    }

    public void ThrowEnemyAtBarrier()
    {
        DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void OnSongListening(Song song)
    {
        if (song is WeakSong weakSong)
        {
            currentSpeed = moveSpeed * weakSong.SlownessFactor;
        }
    }
}