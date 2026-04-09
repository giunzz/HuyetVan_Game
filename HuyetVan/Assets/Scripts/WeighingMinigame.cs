using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class WeighingMiniGame : MonoBehaviour
{
    public static WeighingMiniGame Instance;

    [Header("UI")]
    public GameObject puzzleCanvas; // Bảng chứa thanh trượt
    public GameObject successPanel; // Bảng hiện chữ khi thắng
    public TextMeshProUGUI successText;
    public TextMeshProUGUI timerText;

    [Header("Weighing Logic")]
    public Slider weightSlider;
    public float speed = 0.8f;
    public float targetMin = 0.45f;
    public float targetMax = 0.55f;

    [Header("Swap Model (Tùy chọn)")]
    public GameObject rawFruitsModel;   // Trái cây lộn xộn trên bàn
    public GameObject packedBagModel;   // Túi hàng đã được đóng gói xong

    [Header("Text")]
    [TextArea]
    public string message = "Cân chuẩn xác! Đang đóng gói hàng...";
    public float typingSpeed = 0.05f;

    [Header("Time")]
    public float timeLimit = 15f; // Chỉ cho 15 giây để canh nhịp

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

        // 1. Chạy thời gian
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            UpdateTimer();

            if (!isSolved)
            {
                Debug.Log("⏰ Hết giờ! Khách hàng tức giận bỏ đi...");
                ResetPuzzle();
            }
            return;
        }
        UpdateTimer();

        // 2. Chạy thanh trượt
        weightSlider.value = Mathf.PingPong(Time.time * speed, 1f);

        // 3. Nhận phím Space để chốt cân
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckWeight();
        }
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
        puzzleCanvas.SetActive(true);
        if (successPanel != null) successPanel.SetActive(false);
        
        if (rawFruitsModel != null) rawFruitsModel.SetActive(true);   // Bày trái cây ra
        if (packedBagModel != null) packedBagModel.SetActive(false);  // Giấu cái túi đi

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPlaying = true;
        isSolved = false;
        currentTime = timeLimit;
        UpdateTimer();
    }

    void CheckWeight()
    {
        if (weightSlider.value >= targetMin && weightSlider.value <= targetMax)
        {
            OnPuzzleSolved();
        }
        else
        {
            // Trừ 2 giây nếu bấm sai cho thêm phần kịch tính
            currentTime -= 2f; 
            Debug.Log("Cân sai! Bị phạt trừ 2 giây!");
        }
    }

    public void OnPuzzleSolved()
    {
        isSolved = true;
        isPlaying = false;

        StartCoroutine(SwapModel());

        if (successPanel != null)
        {
            successPanel.SetActive(true);
            if (successText != null) StartCoroutine(TypeWriter());
        }
    }

    IEnumerator SwapModel()
    {
        // Đợi 0.5s rồi tráo mô hình trái cây thành túi đồ đã gói
        yield return new WaitForSeconds(0.5f);

        if (packedBagModel != null) packedBagModel.SetActive(true);
        if (rawFruitsModel != null) rawFruitsModel.SetActive(false);
    }

    IEnumerator TypeWriter()
    {
        successText.text = "";
        foreach (char c in message)
        {
            successText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Đợi người chơi đọc chữ xong (khoảng 2 giây) rồi tắt bảng, báo cho StoryManager
        yield return new WaitForSeconds(2f);
        CloseMiniGame();
        
        // Gọi thẳng về hệ thống cốt truyện để đi tiếp
        if (StoryManager.Instance != null)
        {
            StoryManager.Instance.MinigameSuccess();
        }
    }

    void ResetPuzzle()
    {
        isPlaying = false;
        CloseMiniGame();
        // Có thể gọi một hàm thất bại bên StoryManager nếu bạn muốn (VD: StoryManager.Instance.GameOver())
    }

    public void CloseMiniGame()
    {
        puzzleCanvas.SetActive(false);
        if (successPanel != null) successPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPlaying = false;
    }
}