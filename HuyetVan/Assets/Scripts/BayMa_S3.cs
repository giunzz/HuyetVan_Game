using UnityEngine;
using System.Collections; // Bắt buộc có dòng này để xài bộ đếm thời gian
using TMPro;
using UnityEngine.SceneManagement;

public class BayMa_S3 : MonoBehaviour
{
    public GameObject hinhMaUI;       // Kéo cái Hinh_Ma_UI vào đây
    public GameObject manHinhEnding;  // Kéo cái ManHinh_Ending_UI vào đây
    
    private Balo baloNhanVat;
    private bool daKichHoat = false;  // Tránh hù 2 lần liên tục
    [Header("Scene Transition")]
    public string nextSceneName = "00_Home";
    public float delayBeforeLoad = 3f;

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
        // 1. Hiện jumpscare
        hinhMaUI.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        // 2. Hiện ending
        hinhMaUI.SetActive(false);
        manHinhEnding.SetActive(true);

        // 3. Dừng game
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ❗ DÙNG realtime vì timeScale = 0
        yield return new WaitForSecondsRealtime(delayBeforeLoad);

        // 4. Load scene
        Time.timeScale = 1f; // reset lại trước khi load
        SceneManager.LoadScene(nextSceneName);
    }
}