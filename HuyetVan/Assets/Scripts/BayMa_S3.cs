using UnityEngine;
using System.Collections; // Bắt buộc có dòng này để xài bộ đếm thời gian
using TMPro;

public class BayMa_S3 : MonoBehaviour
{
    public GameObject hinhMaUI;       // Kéo cái Hinh_Ma_UI vào đây
    public GameObject manHinhEnding;  // Kéo cái ManHinh_Ending_UI vào đây
    
    private Balo baloNhanVat;
    private bool daKichHoat = false;  // Tránh hù 2 lần liên tục

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem có phải NVC không VÀ bẫy chưa sập
        if (other.CompareTag("Player") && !daKichHoat)
        {
            baloNhanVat = other.GetComponent<Balo>();

            // CHỈ HÙ KHI NVC ĐÃ LẤY ĐƯỢC BẢN ĐỒ
            if (baloNhanVat != null && baloNhanVat.coBanDo == true)
            {
                daKichHoat = true;
                StartCoroutine(KichHoatJumpscare()); // Khởi động chuỗi hiệu ứng
            }
        }
    }

    // Chuỗi kịch bản đạo diễn:
    IEnumerator KichHoatJumpscare()
    {
        // 1. Phóng mặt con ma che kín màn hình
        hinhMaUI.SetActive(true);
        // (Lưu ý: Sau này có âm thanh hét thì code lệnh Play ở đây)

        // 2. Dừng thời gian đợi 1.5 giây cho não người chơi xử lý cú shock
        yield return new WaitForSeconds(1.5f);

        // 3. Tắt mặt ma, Sập màn hình đen Ending xuống
        hinhMaUI.SetActive(false);
        manHinhEnding.SetActive(true);

        // 4. Khóa cứng game lại (Đóng băng mọi thứ)
        Time.timeScale = 0f; 
        
        // Giải phóng con trỏ chuột ra để người chơi tự tắt game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}