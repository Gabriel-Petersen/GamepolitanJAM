using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void OnTryAgainClick()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
