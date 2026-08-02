using UnityEngine;

public class EnemyDamageArea : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private Vector2 pushForce;
    [SerializeField] private float attackDuration = 0.5f;

    private float lastUsedTime = 0f;
    private bool isAttacking = false;
    private bool attackConfirmed = false;
    private float attackStartTime = 0f;

    void Start()
    {
        lastUsedTime = -cooldownTime;
    }

    void Update()
    {
        if (lastUsedTime + cooldownTime < Time.time && !isAttacking)
        {
            StartAttack();
        }

        if (attackStartTime + attackDuration < Time.time && isAttacking)
        {
            EndAttack();
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (isAttacking && !attackConfirmed)
        {
            GameObject obj = collision.gameObject;

            if (obj.CompareTag("Player"))
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
        if (target.gameObject.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.PushPlayer(pushDirection, pushForce);
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackStartTime = Time.time;
    }

    private void EndAttack()
    {
        attackConfirmed = false;
        isAttacking = false;
        lastUsedTime = Time.time;
    }
}