using UnityEngine;

public class WeakSong : Song
{
    [SerializeField] private float maxRadius;
    [SerializeField] private float growDuration;
    [SerializeField] private float shrinkDuration;

    public float slownessFactor;

    private float currentRadius = 0;

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
}
