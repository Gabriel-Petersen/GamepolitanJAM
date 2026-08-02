using UnityEngine;

public class BarreiraDestruidaComInimigo : Barrier
{
    [SerializeField] private float shakeIntensity;

    private Vector3 originalPosition;

    public float velocityBreakThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {

        if(collision.rigidbody.linearVelocity.sqrMagnitude > velocityBreakThreshold * velocityBreakThreshold)
        {
            // throwEnemy is available here if needed
            BreakWall(collision);
        }
        
    }

    public void BreakWall(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<EnemyAi>(out var enemy))
        {
            enemy.ThrowEnemyAtBarrier();
        }
        Destroy(gameObject);
        onBreakEvent?.Invoke();
    }

    public override void OnSongListening(Song song)
    {
        if (song is WeakSong)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = originalPosition + randomOffset;
        }
    }
}
