using UnityEngine;

public class WeakBarrierManager : MonoBehaviour
{
    public WeakBarrier[] WeakBarriers { get; private set; }

    private void Start()
    {
        WeakBarriers = FindObjectsByType<WeakBarrier>(FindObjectsSortMode.None);
    }
}
