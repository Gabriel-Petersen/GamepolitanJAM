using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NpcManager : MonoBehaviour
{
    [SerializeField] private string winSceneName;
    [SerializeField] private UnityEvent onAllNpcsHappyEvent;
    [SerializeField] private UnityEvent onEachNpcBecomeHappyEvent;
    private Npc[] npcs;
    public int HappyNpcCounter {  get; private set; }
    public int NpcCounter => npcs.Length;

    private void Start()
    {
        npcs = FindObjectsByType<Npc>(FindObjectsSortMode.None);
        HappyNpcCounter = 0;

        foreach (Npc npc in npcs)
        {
            npc.OnBecameHappyEvent.AddListener(AddHappyNpcToCounter);
        }
    }

    private void AddHappyNpcToCounter()
    {
        onEachNpcBecomeHappyEvent.Invoke();
        if (++HappyNpcCounter >= NpcCounter)
        {
            onAllNpcsHappyEvent.Invoke();
            ToWinScene();
        }
    }

    private void ToWinScene()
    {
        SceneManager.LoadScene(winSceneName);
    }
}
