using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class WeakSong : Song
{
    [SerializeField] private float maxRadius;
    [SerializeField] private float growDuration;
    [SerializeField] private float shrinkDuration;
    [SerializeField] private GameObject weakNote;

    public float slownessFactor;

    public float currentRadius = 0;
    public float instantiateCooldown = 0.1f;
    public float lastInstantiateTime;
    public float forwardOffset = 1f;

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
                Debug.Log(collider.name + " is within the weak song radius.");

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
    }

    private void SpawnWeakNote()
    {
        GameObject note = Instantiate(weakNote, transform.position + transform.forward * forwardOffset, Quaternion.identity);
        Vector3 noteDirection = Random.insideUnitSphere;
        
        if(note.TryGetComponent<WeakNote>(out var wn)){
            if (wn != null)
            {
                wn.weakSong = this;
                wn.velocity = noteDirection.normalized;
            }
        }


        float radius = 0f;
        
    }   
}
