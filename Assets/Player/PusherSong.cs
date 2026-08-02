using UnityEngine;
using UnityEngine.Audio;

public class PusherSong : Song
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PusherSongNote notePrefab;

    [Header("Efeito Visual do Toro")]
    [SerializeField] private GameObject torusPrefab;

    [Header("Pulso Circular (Esfera Curta)")]
    [SerializeField] private float sphereRadius = 3f;
    [SerializeField] private float sphereInterval = 0.5f;
    [SerializeField] private float spherePushDuration = 0.15f;
    [SerializeField] private Vector2 spherePushForce;
    [SerializeField] private int sphereNoteCount = 5;

    [Header("Pulso Direcional (Setor Esférico Frontal)")]
    [SerializeField] private float coneRange = 7f;
    [SerializeField, Range(10f, 180f)] private float coneAngle = 90f;
    [SerializeField] private float coneInterval = 1.2f;
    [SerializeField] private float conePushDuration = 0.3f;
    [SerializeField] private Vector2 conePushForce;
    [SerializeField] private int coneNoteCount = 10;

    [Header("Debug")]
    [SerializeField] private bool debug = true;

    private float nextSphereTime = 0f;
    private float nextConeTime = 0f;
    private bool isSingingActive = false;
    public AudioSource audioSource;

    private void Update()
    {
        isSingingActive = Input.GetMouseButton(0);

        if (isSingingActive)
        {
            if (Time.time >= nextSphereTime)
            {
                ExecuteSpherePulse();
                audioSource.Play();
                nextSphereTime = Time.time + sphereInterval;
            }

            if (Time.time >= nextConeTime)
            {
                ExecuteConePulse();
                audioSource.Play();
                nextConeTime = Time.time + coneInterval;
            }
        }
    }

    private void ExecuteSpherePulse()
    {
        SpawnSphereNotes(sphereNoteCount);

        // Para o pulso circular, o toro expande em todas as direções a partir do centro (usamos transform.forward como base)
        SpawnTorusEffect(transform.position + Vector3.up * 0.5f, transform.forward, sphereRadius, spherePushDuration);

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
        SpawnConeNotes(coneNoteCount);

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        // Para o pulso direcional, o toro avança alinhado para a frente do cone
        SpawnTorusEffect(transform.position + Vector3.up * 0.5f, forward, coneRange, conePushDuration);

        Collider[] colliders = Physics.OverlapSphere(transform.position, coneRange, enemyLayer);
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

    private void SpawnTorusEffect(Vector3 origin, Vector3 direction, float targetRadius, float duration)
    {
        if (torusPrefab == null) return;

        var torusObj = Instantiate(torusPrefab);

        if (torusObj.TryGetComponent<WaveTorusEffect>(out var torusEffect))
        {
            torusEffect.Initialize(origin, direction, targetRadius, duration);
        }
    }

    private void ApplyPushToEnemy(Collider enemyCollider, Vector3 direction, Vector2 force, float duration)
    {
        if (enemyCollider.TryGetComponent<EnemyAi>(out var enemy))
        {
            enemy.TriggerKnockback(direction, force, duration);
        }
    }

    private void SpawnSphereNotes(int count)
    {
        if (notePrefab == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
            var note = Instantiate(notePrefab, spawnPos, Quaternion.identity);

            Vector3 randomDir = Random.insideUnitSphere;
            randomDir.y = Mathf.Abs(randomDir.y) * 0.5f;

            note.sourceTransform = transform;
            note.maxDistance = sphereRadius;
            note.velocity = randomDir.normalized;
        }
    }

    private void SpawnConeNotes(int count)
    {
        if (notePrefab == null) return;

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        for (int i = 0; i < count; i++)
        {
            float randomYaw = Random.Range(-coneAngle * 0.5f, coneAngle * 0.5f);
            float randomPitch = Random.Range(-10f, 25f);

            Quaternion rot = Quaternion.Euler(randomPitch, randomYaw, 0);
            Vector3 spreadDir = rot * forward;

            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
            var note = Instantiate(notePrefab, spawnPos, Quaternion.identity);

            if (note is PusherSongNote pusherNote)
            {
                pusherNote.sourceTransform = transform;
                pusherNote.maxDistance = coneRange;
                pusherNote.velocity = spreadDir.normalized;
            }
        }
    }

    public override bool IsSinging() => isSingingActive;
    public override bool IsEnabled() => isSingingActive;

    private void OnDrawGizmos()
    {
        if (!debug) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, coneRange);

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Gizmos.color = Color.magenta;
        Quaternion leftRot = Quaternion.Euler(0, -coneAngle * 0.5f, 0);
        Quaternion rightRot = Quaternion.Euler(0, coneAngle * 0.5f, 0);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftDir * coneRange);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * coneRange);
    }
}