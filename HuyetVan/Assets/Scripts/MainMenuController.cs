using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    public string morgueSceneName = "morgue"; // scene morgue của bạn

    [Header("UI")]
    public GameObject readmePanel;

    // ▶️ NEW GAME
    public void NewGame()
    {
        Debug.Log("▶️ Start Game");
        SceneManager.LoadScene(morgueSceneName);
    }

    // ❌ EXIT GAME
    public void ExitGame()
    {
        Debug.Log("❌ Quit Game");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // 📖 OPEN README
    public void OpenReadme()
    {
        if (readmePanel != null)
            readmePanel.SetActive(true);
    }

    // ❌ CLOSE README
    public void CloseReadme()
    {
        if (readmePanel != null)
            readmePanel.SetActive(false);
    }
}