using UnityEngine;
using UnityEngine.SceneManagement; // Cần cái này để load lại màn chơi
using TMPro;

public class CaiGieng_S3 : MonoBehaviour
{
    public TextMeshProUGUI vungChu;
    public GameObject chiaKhoaPhongCuoi; // Kéo cái chìa khóa đã tắt vào đây
    private bool dungGan = false;
    private Balo baloNhanVat;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dungGan = true;
            baloNhanVat = other.GetComponent<Balo>();
            vungChu.gameObject.SetActive(true);
            vungChu.text = "Bấm [E] để thực hiện nghi thức ...";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dungGan = false;
            vungChu.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (dungGan && Input.GetKeyDown(KeyCode.E))
        {
            // KIỂM TRA ĐIỀU KIỆN 9 MẢNH GƯƠNG
            if (baloNhanVat.soManhGuong < 9)
            {
                // PHẠT: Tua ngược thời gian (Load lại màn chơi)
                Debug.Log("Bạn chưa đủ linh hồn mảnh gương! Tua ngược...");
                RestartLevel();
            }
            else
            {
                // THƯỞNG: Cho phép lấy chìa khóa
                Debug.Log("Mảnh gương đã hợp nhất! Chìa khóa xuất hiện.");
                vungChu.text = "Bạn tìm thấy chìa khóa trên miệng giếng!";
                chiaKhoaPhongCuoi.SetActive(true); // Làm cái chìa khóa hiện ra
            }
        }
    }

    void RestartLevel()
    {
        // Lấy tên Scene hiện tại và load lại nó
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}