using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private float fakeLoadingTime;

    private void Start()
    {
        menuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        try {
            Invoke(nameof(LoadGameScene), fakeLoadingTime);
            gameObject.SetActive(false);
        }
        catch (System.Exception e) {
            Debug.LogError($"Failed to load scene: {e.Message}");
        }
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnCreditsButtonClicked() {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnExitButtonClicked() {
        Application.Quit();
    }

    public void OnReturnButtonClicked() {
        creditsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
