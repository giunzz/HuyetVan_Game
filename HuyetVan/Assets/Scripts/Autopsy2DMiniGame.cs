using UnityEngine;
using System.Collections;
using TMPro;

public class Autopsy2DMiniGame : MonoBehaviour
{
    public static Autopsy2DMiniGame Instance;

    [Header("UI")]
    public GameObject puzzleCanvas;
    public GameObject successPanel;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI timerText;

    [Header("Anchor")]
    public Transform bodyAnchor;

    [Header("Puzzle")]
    public GameObject puzzleManager;

    [Header("Swap Body")]
    public GameObject morgueFinal;   // xác cũ
    public GameObject newBodyModel;  // ✅ FIX: thêm biến này

    [Header("Text")]
    [TextArea]
    public string message = "Tuyệt vời! Bạn đã tìm ra nguyên nhân...";
    public float typingSpeed = 0.05f;

    [Header("Time")]
    public float timeLimit = 300f;

    private float currentTime;
    private bool isPlaying = false;
    private bool isSolved = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isPlaying) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            UpdateTimer();

            if (!isSolved)
            {
                Debug.Log("⏰ Hết giờ!");
                ResetPuzzle();
            }

            isPlaying = false;
            return;
        }

        UpdateTimer();
    }

    void UpdateTimer()
    {
        if (timerText == null) return;

        int min = Mathf.FloorToInt(currentTime / 60);
        int sec = Mathf.FloorToInt(currentTime % 60);

        timerText.text = $"{min:00}:{sec:00}";
    }

    public void OpenMiniGame()
    {
        Debug.Log("OPEN MINIGAME");

        puzzleCanvas.SetActive(true);

        if (successPanel != null)
            successPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPlaying = true;
        isSolved = false;

        currentTime = timeLimit;
        UpdateTimer();
    }

    public void OnPuzzleSolved()
    {
        Debug.Log("✅ Đã xếp xong!");

        isSolved = true;
        isPlaying = false;

        StartCoroutine(SwapBody());

        if (successPanel != null)
        {
            successPanel.SetActive(true);

            if (successText != null)
                StartCoroutine(TypeWriter());
        }
    }

    IEnumerator SwapBody()
    {
        yield return new WaitForSeconds(1f);

        if (newBodyModel == null)
        {
            Debug.LogError("❌ Missing newBodyModel");
            yield break;
        }

        newBodyModel.SetActive(true);

        if (morgueFinal != null)
            morgueFinal.SetActive(false);
    }

    IEnumerator TypeWriter()
    {
        successText.text = "";

        foreach (char c in message)
        {
            successText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void ResetPuzzle()
    {
        Debug.Log("RESET PUZZLE");

        isPlaying = false;

        if (puzzleManager != null)
        {
            puzzleManager.SetActive(false);
            puzzleManager.SetActive(true);
        }

        if (successPanel != null)
            successPanel.SetActive(false);

        CloseMiniGame();
    }

    public void CloseMiniGame()
    {
        puzzleCanvas.SetActive(false);

        if (successPanel != null)
            successPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPlaying = false;
    }
}