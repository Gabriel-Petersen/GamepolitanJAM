using UnityEngine;

public class EnemyAttackNote : SongNote
{
    [HideInInspector] public Transform sourceTransform;
    [SerializeField] private float maxTravelDistance = 4f;

    public override bool ShouldBeDestroyed()
    {
        if (sourceTransform == null) return true;

        return (transform.position - sourceTransform.position).sqrMagnitude > maxTravelDistance * maxTravelDistance;
    }
}