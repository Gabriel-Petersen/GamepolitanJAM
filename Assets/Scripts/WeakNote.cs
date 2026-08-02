using UnityEngine;

public class WeakNote : SongNote
{
    [HideInInspector] public WeakSong weakSong;

    public override bool ShouldBeDestroyed()
    {
        if (weakSong == null) return true;

        float currentDynamicRadius = weakSong.CurrentRadius;
        return (transform.position - weakSong.transform.position).sqrMagnitude > currentDynamicRadius * currentDynamicRadius;
    }
}