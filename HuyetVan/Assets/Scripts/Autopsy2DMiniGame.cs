using UnityEngine;
using UnityEngine.UI;

public class Autopsy2DMiniGame : MonoBehaviour
{
    public static Autopsy2DMiniGame Instance;

    [Header("UI Components")]
    public GameObject miniGamePanel; // Khung Panel đen che màn hình
    public Slider cutSlider;         // Thanh Slider (Vết rạch)

    [Header("Phần Thưởng (Manh mối)")]
    public GameObject cluePrefab;    // Kéo Prefab chiếc chìa khóa vào đây
    public Transform spawnPoint;     // Kéo vị trí muốn rơi chìa khóa vào đây (ví dụ: trên bàn mổ)

    private bool _isCompleted = false;

    void Awake()
    {
        // Khởi tạo Singleton chuẩn chống trùng lặp
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (miniGamePanel != null) miniGamePanel.SetActive(false);
    }

    // Hàm này gọi khi bấm E vào cái xác
    public void OpenMiniGame()
    {
        // Nếu đã mổ xong rồi thì không cho mổ lại nữa
        if (_isCompleted)
        {
            if (MonologueManager.Instance != null)
                MonologueManager.Instance.Show("Mình đã khám nghiệm cái xác này xong rồi.");
            return;
        }

        cutSlider.value = 0; // Đặt dao mổ về vạch xuất phát
        miniGamePanel.SetActive(true);
        
        // Mở khóa chuột để người chơi cầm chuột kéo UI 2D
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (MonologueManager.Instance != null)
            MonologueManager.Instance.Show("Hãy dùng chuột kéo dao mổ theo đường đứt nét...");
    }

    // Hàm này kiểm tra liên tục mỗi khi người chơi kéo con dao (Slider)
    public void CheckProgress()
    {
        if (_isCompleted) return;

        // Nếu kéo chạm tới đáy (value = 1 là 100%)
        if (cutSlider.value >= 0.99f)
        {
            _isCompleted = true;
            WinMiniGame();
        }
    }

    void WinMiniGame()
    {
        Debug.Log("✅ Phẫu thuật thành công!");
        miniGamePanel.SetActive(false); // Đóng UI mini game
        
        // Khóa chuột lại để tiếp tục chơi góc nhìn thứ nhất (3D)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (MonologueManager.Instance != null)
            MonologueManager.Instance.Show("Xong rồi. Để xem trong dạ dày có giấu thứ gì không...");

        // Đẻ ra (Spawn) chiếc chìa khóa hoặc manh mối rớt xuống bàn mổ
        if (cluePrefab != null && spawnPoint != null)
        {
            Instantiate(cluePrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("🔑 Đã rớt ra manh mối!");
        }
        else
        {
            Debug.LogWarning("⚠️ Chưa gán Prefab manh mối hoặc vị trí rớt trong Inspector!");
        }
    }
}