using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class endload : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "MARKET";

    void Start()
    {
        // đăng ký sự kiện khi video kết thúc
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}