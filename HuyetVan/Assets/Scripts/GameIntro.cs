using UnityEngine;
using System.Collections;
using TMPro;

public class GameIntro : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI subtitleText;
    public CanvasGroup fadePanel;

    [Header("Player Control")]
    public GameObject player;

    string[] dialogues = new string[]
    {
        "Ca thứ ba trong tuần. Hồ sơ ghi: Nữ, khoảng 30 tuổi, vô gia cư.\nTử vong do suy tim đột ngột.",
        "Bắt đầu thôi. Xong sớm để còn về..."
    };

    private CharacterController _cc;
    private PlayerMovement _pm;
    private Vector3 _playerStartPos;
    private Quaternion _playerStartRot;

    void Start()
    {
        if (player != null)
        {
            _cc  = player.GetComponent<CharacterController>();
            _pm  = player.GetComponent<PlayerMovement>();
            _playerStartPos = player.transform.position;
            _playerStartRot = player.transform.rotation;
        }

        DisablePlayer(true);

        if (subtitleText != null) subtitleText.text = "";
        if (fadePanel != null)    fadePanel.alpha = 1f;

        StartCoroutine(PlayIntro());
    }

    void DisablePlayer(bool disable)
    {
        if (player == null) return;

        if (_cc != null) _cc.enabled = false;

        foreach (var script in player.GetComponentsInChildren<MonoBehaviour>())
        {
            if (script != this) script.enabled = !disable;
        }

        if (!disable && _cc != null)
            _cc.enabled = true;
    }

    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeIn(3f));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(TypewriterEffect(dialogues[0], 0.06f));
        yield return new WaitForSeconds(3f);
        subtitleText.text = "";

        yield return StartCoroutine(TypewriterEffect(dialogues[1], 0.06f));
        yield return new WaitForSeconds(3f);
        subtitleText.text = "";

        yield return StartCoroutine(EnablePlayerSafe());

        Debug.Log("✅ Intro xong!");
    }

    IEnumerator EnablePlayerSafe()
    {
        // Bước 1: tắt CC, reset vị trí & rotation
        if (_cc != null) _cc.enabled = false;
        player.transform.position = _playerStartPos;
        player.transform.rotation = _playerStartRot;

        yield return null;

        // Bước 2: bật CC nhưng CHƯA bật PlayerMovement
        if (_cc != null) _cc.enabled = true;
        if (_pm != null) _pm.enabled = false;

        // Bước 3: dùng CC tự xử lý gravity để player rơi xuống sàn
        Vector3 fallVelocity = Vector3.zero;
        float timeout = 2f; // tối đa chờ 2 giây
        float elapsed = 0f;

        while (!_cc.isGrounded && elapsed < timeout)
        {
            fallVelocity.y += Physics.gravity.y * Time.deltaTime;
            _cc.Move(fallVelocity * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Bước 4: player đã chạm sàn → enable toàn bộ
        if (_pm != null) _pm.enabled = true;

        // Bật các script khác
        foreach (var script in player.GetComponentsInChildren<MonoBehaviour>())
        {
            if (script != this && script != _pm)
                script.enabled = true;
        }

        Debug.Log($"✅ Player chạm sàn sau {elapsed:F2}s");
    }

    IEnumerator TypewriterEffect(string text, float delay)
    {
        if (subtitleText == null)
        {
            Debug.LogError("❌ subtitleText chưa gán!");
            yield break;
        }
        subtitleText.text = "";
        foreach (char c in text)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator FadeIn(float duration)
    {
        if (fadePanel == null)
        {
            Debug.LogError("❌ fadePanel chưa gán!");
            yield break;
        }
        float elapsed = 0f;
        while (elapsed < duration)
        {
            fadePanel.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadePanel.alpha = 0f;
    }
}