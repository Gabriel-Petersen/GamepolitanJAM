using TMPro;
using UnityEngine;

public class happyScore : MonoBehaviour
{ 


    [SerializeField] private TextMeshProUGUI numberText;
    public NpcManager npcManager;

    private int currentNumber = 0;

    private void Update()
    {
        // Update the current number based on the number of happy NPCs
        currentNumber = npcManager.HappyNpcCounter;
        UpdateUI();
    }
    // Updates the visual UI text
    private void UpdateUI()
    {
        numberText.text = currentNumber.ToString();
    }

}
