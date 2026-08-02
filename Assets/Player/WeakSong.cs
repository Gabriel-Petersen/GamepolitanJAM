using UnityEngine;
using UnityEngine.UIElements;

public class WeakSong : Song
{
    [SerializeField] private float maxRadius;
    [SerializeField] private float growDuration;
    [SerializeField] private float shrinkDuration;
    [SerializeField] private WeakNote weakNote;
    [SerializeField] private float instantiateCooldown = 0.1f;
    [SerializeField] private float forwardOffset = 1f;
    [SerializeField] private float slownessFactor;


    [Header(("Debug"))]
    [SerializeField] private bool debugMaxRadius;

    private float currentRadius = 0;
    private float lastInstantiateTime;
    public float SlownessFactor => slownessFactor;
    public float CurrentRadius => currentRadius;

    private void Update()
    {
        if (IsSinging())
        {
            float growSpeed = maxRadius / growDuration;
            currentRadius += growSpeed * Time.deltaTime;
        }
        else
        {
            float shrinkSpeed = maxRadius / shrinkDuration;
            currentRadius -= shrinkSpeed * Time.deltaTime;
        }

        currentRadius = Mathf.Clamp(currentRadius, 0f, maxRadius);

        if (currentRadius > 0f)
        {

            if(lastInstantiateTime + instantiateCooldown < Time.time)
            {
                SpawnWeakNote();
                lastInstantiateTime = Time.time;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, currentRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<ISongResponsive>(out var songResponsive))
                {
                    songResponsive.OnSongListening(this);
                }
            }
        }
    }

    public override bool IsSinging()
    {
        return Input.GetMouseButton(1); // Left mouse button
    }
    
    public override bool IsEnabled()
    {
        return currentRadius > 0;
    }

    private void OnDrawGizmos()
    {
        if (currentRadius > 0f)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, currentRadius);
        }
        else if (debugMaxRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxRadius);
        }
    }

    private void SpawnWeakNote()
    {
        var note = Instantiate(weakNote, transform.position + transform.forward * forwardOffset, Quaternion.identity);
        Vector3 noteDirection = Random.insideUnitSphere + transform.forward / 10.0f;

        int spriteIndex = Random.Range(0, note.sprites.Count);
        note.spriteRenderer.sprite = note.sprites[spriteIndex];
        note.weakSong = this;
        note.velocity = noteDirection.normalized;        
    }   
}
