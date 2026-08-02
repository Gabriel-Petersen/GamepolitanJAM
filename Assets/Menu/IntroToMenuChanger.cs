using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class IntroToMenuChanger : MonoBehaviour
{
    [SerializeField] private string menuSceneName;

    private void Awake()
    {
        GetComponent<VideoPlayer>().loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer source)
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
