using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenuController : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "01_Intro";

    public void OnNewGame()
    {
        SceneManager.LoadScene(firstSceneName);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}