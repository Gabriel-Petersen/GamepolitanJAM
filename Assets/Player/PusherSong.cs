using UnityEngine;

public class PusherSong : Song
{
    [SerializeField] private LayerMask enemyLayer;

    [Header("Pulso Circular (Esfera Curta)")]
    [SerializeField] private float sphereRadius = 3f;
    [SerializeField] private float sphereInterval = 0.5f;
    [SerializeField] private float spherePushDuration = 0.15f;
    [SerializeField] private Vector2 spherePushForce;

    [Header("Pulso Direcional (Setor Esférico Frontal)")]
    [SerializeField] private float coneRange = 7f;
    [SerializeField, Range(10f, 180f)] private float coneAngle = 90f;
    [SerializeField] private float coneInterval = 1.2f;
    [SerializeField] private float conePushDuration = 0.3f;
    [SerializeField] private Vector2 conePushForce;

    [Header("Debug")]
    [SerializeField] private bool debug = true;

    private float sphereTimer = 0f;
    private float coneTimer = 0f;
    private bool isSingingActive = false;

    private void Update()
    {
        isSingingActive = Input.GetMouseButton(0);

        if (isSingingActive)
        {
            float deltaTime = Time.deltaTime;
            sphereTimer += deltaTime;
            coneTimer += deltaTime;

            if (sphereTimer >= sphereInterval)
            {
                ExecuteSpherePulse();
                sphereTimer = 0f;
            }

            if (coneTimer >= coneInterval)
            {
                ExecuteConePulse();
                coneTimer = 0f;
            }
        }
        else
        {
            sphereTimer = sphereInterval;
            coneTimer = coneInterval;
        }
    }

    private void ExecuteSpherePulse()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereRadius, enemyLayer);
        foreach (Collider col in colliders)
        {
            Vector3 pushDir = (col.transform.position - transform.position).normalized;
            pushDir.y = 0f;

            if (col.TryGetComponent<ISongResponsive>(out var songResponsive))
            {
                songResponsive.OnSongListening(this);
            }

            ApplyPushToEnemy(col, pushDir, spherePushForce, spherePushDuration);
        }
    }

    private void ExecuteConePulse()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, coneRange, enemyLayer);
        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        float halfAngleCos = Mathf.Cos((coneAngle * 0.5f) * Mathf.Deg2Rad);

        foreach (Collider col in colliders)
        {
            Vector3 targetDir = (col.transform.position - transform.position);
            float distanceToTarget = targetDir.magnitude;
            targetDir.y = 0f;
            targetDir.Normalize();

            bool isVeryClose = distanceToTarget <= sphereRadius;

            if (isVeryClose || Vector3.Dot(forward, targetDir) >= halfAngleCos)
            {
                Vector3 pushDir = isVeryClose ? targetDir : forward;
                ApplyPushToEnemy(col, pushDir, conePushForce, conePushDuration);
            }

            if (col.TryGetComponent<ISongResponsive>(out var songResponsive))
            {
                songResponsive.OnSongListening(this);
            }
        }
    }

    private void ApplyPushToEnemy(Collider enemyCollider, Vector3 direction, Vector2 force, float duration)
    {
        if (enemyCollider.TryGetComponent<EnemyAi>(out var enemy))
        {
            enemy.TriggerKnockback(direction, force, duration);
        }
    }

    public override bool IsSinging()
    {
        return isSingingActive;
    }

    public override bool IsEnabled()
    {
        return isSingingActive;
    }

    private void OnDrawGizmos()
    {
        if (!debug) return;

        // 1. Gizmo do Pulso Circular (Esfera - Cor Ciano)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);

        // 2. Gizmos do Setor Esférico Frontal (Cone - Cor Amarela)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, coneRange);

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        // Desenha as linhas delimitadoras do ângulo de visão/empurro frontal
        Gizmos.color = Color.magenta;
        Quaternion leftRot = Quaternion.Euler(0, -coneAngle * 0.5f, 0);
        Quaternion rightRot = Quaternion.Euler(0, coneAngle * 0.5f, 0);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftDir * coneRange);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * coneRange);
    }
}