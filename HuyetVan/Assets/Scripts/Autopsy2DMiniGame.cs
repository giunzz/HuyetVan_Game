using UnityEngine;

public class Autopsy2DMiniGame : MonoBehaviour
{
   
    public static Autopsy2DMiniGame Instance;

    [Header("Tham chiếu")]
    public GameObject puzzleCanvas;
    public GameObject puzzleManager;

    void Awake()
    {
        // Khởi tạo đường dây nóng ngay khi vào game
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 2. HÀM MỞ MINI GAME
    public void OpenMiniGame()
    {
        Debug.Log("Đã gọi hàm OpenMiniGame thành công!"); // In ra để kiểm tra

        if (puzzleCanvas != null)
        {
            // Bật cái Canvas xám xịt đó lên!
            puzzleCanvas.SetActive(true);

            // Mở khóa chuột và hiện chuột lên để người chơi xếp hình
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("⚠️ Chưa gán Puzzle Canvas trong Inspector kìa!");
        }
    }

    // (Hàm này dùng để tắt game sau khi xếp hình xong)
    public void CloseMiniGame()
    {
        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false); // Tắt Canvas đi
            
            // Khóa chuột lại để chơi góc nhìn thứ nhất (FPS)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}