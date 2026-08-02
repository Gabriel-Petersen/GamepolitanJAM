using UnityEngine;
using UnityEngine.Events;

public abstract class Barrier : MonoBehaviour, ISongResponsive
{
    [SerializeField] protected UnityEvent onBreakEvent;
    public UnityEvent GetOnBreakEvent() { return onBreakEvent; }
    public abstract void OnSongListening(Song song);
}
