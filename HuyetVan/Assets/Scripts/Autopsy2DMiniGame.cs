using UnityEngine;
using System.Collections;
using TMPro; // THÊM DÒNG NÀY ĐỂ GAME HIỂU ĐƯỢC TEXT MESH PRO

public class Autopsy2DMiniGame : MonoBehaviour
{
    public static Autopsy2DMiniGame Instance;

    [Header("Tham chiếu Giao diện")]
    public GameObject puzzleCanvas;
    public GameObject puzzleManager;
    
    [Header("Giao diện Chiến thắng")]
    public GameObject successPanel; 
    public TextMeshProUGUI successText; // Nơi chứa dòng chữ
    
    [TextArea]
    public string message = "Tuyệt vời! Bạn đã tìm ra mảnh ghép quan trọng của thi thể..."; // Nội dung bạn muốn chạy
    public float typingSpeed = 0.05f; // Tốc độ gõ (số càng nhỏ chạy càng nhanh)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenMiniGame()
    {
        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(true);
            
            // Đảm bảo bảng chiến thắng bị tắt khi mới bắt đầu chơi lại
            if (successPanel != null) successPanel.SetActive(false); 
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnPuzzleSolved()
    {
        Debug.Log("Đã xếp xong! Bật bảng thông báo và chạy chữ...");
        if (successPanel != null)
        {
            successPanel.SetActive(true); // Bật cái bảng lên
            
            // Kích hoạt hiệu ứng gõ chữ nếu đã gán Text
            if (successText != null)
            {
                StartCoroutine(TypeWriterEffect());
            }
        }
    }

    // HIỆU ỨNG GÕ TỪNG CHỮ MỘT
    IEnumerator TypeWriterEffect()
    {
        successText.text = ""; // Xóa trắng chữ cũ ban đầu
        foreach (char c in message)
        {
            successText.text += c; // Nhả từng chữ cái ra màn hình
            yield return new WaitForSeconds(typingSpeed); // Nghỉ một chút theo tốc độ đã cài rồi mới nhả chữ tiếp
        }
    }

    public void CloseMiniGame()
    {
        if (puzzleCanvas != null)
        {
            puzzleCanvas.SetActive(false);
            if (successPanel != null) successPanel.SetActive(false);
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}