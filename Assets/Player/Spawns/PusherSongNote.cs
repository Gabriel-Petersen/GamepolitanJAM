using UnityEngine;

public class PusherSongNote : SongNote
{
    [HideInInspector] public Transform sourceTransform;
    [HideInInspector] public float maxDistance = 5f;

    private float lifeTimer = 0f;
    [SerializeField] private float maxLifetime = 1.5f;

    protected override void Update()
    {
        base.Update();
        lifeTimer += Time.deltaTime;
    }

    public override bool ShouldBeDestroyed()
    {
        if (sourceTransform == null) return true;

        bool exceededDistance = (transform.position - sourceTransform.position).sqrMagnitude > maxDistance * maxDistance;
        bool exceededTime = lifeTimer >= maxLifetime;

        return exceededDistance || exceededTime;
    }
}