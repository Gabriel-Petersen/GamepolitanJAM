using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    public WeakBarrier[] WeakBarriers { get; private set; }
    public BarreiraDestruidaComInimigo[] BarriersWithEnemies { get; private set; }

    private void Start()
    {
        WeakBarriers = FindObjectsByType<WeakBarrier>(FindObjectsSortMode.None);
        BarriersWithEnemies = FindObjectsByType<BarreiraDestruidaComInimigo>(FindObjectsSortMode.None);
    }
}
