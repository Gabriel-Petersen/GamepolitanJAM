using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField] private Barrier barrier;
    [SerializeField] private List<EnemyAi> enemiesAttached;
    [SerializeField] private float hapinessPerSecond = 1.0f;
    [SerializeField] private float maxHapiness = 100.0f;

    [SerializeField] private bool isMaterialChanging;
    [SerializeField] private Material happyMaterial;

    private SongsHolder playerSongsHolder;
    private bool isFree = false;
    private bool isHappy = false;
    private float hapinessProgress = 0;

    private void Start()
    {
        barrier.GetOnBreakEvent().AddListener(BecameFree);
        playerSongsHolder = FindAnyObjectByType<SongsHolder>();
        if (playerSongsHolder == null)
        {
            Debug.LogError("No SongsHolder found in the scene.");
        }

        if (isMaterialChanging && happyMaterial == null)
        {
            Debug.LogWarning($"NPC {gameObject.name} is set to change material, but no happy material is assigned.");
        }
    }

    private void Update()
    {
        if (!isHappy && isFree)
        {
            if (playerSongsHolder.IsAnySongSinging())
            {
                hapinessProgress += hapinessPerSecond * Time.deltaTime;
                if (hapinessProgress >= maxHapiness)
                {
                    hapinessProgress = maxHapiness;
                    Debug.Log($"NPC {gameObject.name} is fully happy!");
                    BecameHappy();
                }
            }
        }
    }

    private void BecameHappy()
    {
        if (isMaterialChanging && happyMaterial != null)
        {
            if (TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material = happyMaterial;
            }
        }

        foreach (var enemy in enemiesAttached)
        {
            if (enemy != null)
                enemy.DestroyEnemy();
        }

        isHappy = true;
    }

    private void BecameFree() 
    { 
        if (isFree)
            return;

        isFree = true;
        barrier = null;
    }
}
