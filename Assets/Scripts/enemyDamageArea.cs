using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDamageArea : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private Vector2 pushForce;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private EnemyAttackNote attackNotePrefab;
    [SerializeField] private int notesPerAttack = 5;
    [SerializeField] private float attackTriggerRadius = 2.5f;

    private float lastUsedTime = 0f;
    private bool isAttacking = false;
    private bool attackConfirmed = false;
    private float attackStartTime = 0f;
    public AudioSource audioSource;
    private Transform playerTransform;

    void Start()
    {
        lastUsedTime = -cooldownTime;
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
     
        if (attackStartTime + attackDuration < Time.time && isAttacking)
        {
            EndAttack();
        }
        /*
        if (playerTransform != null && !isAttacking && lastUsedTime + cooldownTime < Time.time)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) <= attackTriggerRadius)
            {
                StartAttack();
            }
        }
        */
    }

    void OnTriggerStay(Collider collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Player"))
        {
            if (isAttacking && !attackConfirmed)
            {
                if (obj.TryGetComponent<HealthSystem>(out var healthSystem))
                {
                    healthSystem.ChangeHp(-1);
                }
                PushAttack(collision);
                attackConfirmed = true;
            }
        }
    }

    private void PushAttack(Collider target)
    {
        Vector3 pushDirection = (target.transform.position - transform.position).normalized;
        pushDirection.y = 0f;

        if (target.gameObject.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.PushPlayer(pushDirection, pushForce);
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        attackStartTime = Time.time;
        SpawnAttackNotes();
        audioSource.Play();
    }

    private void EndAttack()
    {
        attackConfirmed = false;
        isAttacking = false;
        lastUsedTime = Time.time;
    }

    private void SpawnAttackNotes()
    {
        

        if (attackNotePrefab == null || playerTransform == null) return;

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        directionToPlayer.y = 0f;

        for (int i = 0; i < notesPerAttack; i++)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
            var note = Instantiate(attackNotePrefab, spawnPos, Quaternion.identity);

            Vector3 randomSpread = (Random.insideUnitSphere * 0.4f) + directionToPlayer;
            randomSpread.y = Mathf.Abs(randomSpread.y) * 0.3f;

            note.sourceTransform = transform;
            note.velocity = randomSpread.normalized;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackTriggerRadius);
    }
}