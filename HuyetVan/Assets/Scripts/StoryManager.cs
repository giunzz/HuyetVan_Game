using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    [Header("Main UI")]
    [Header("Scene Transition")]
    public string nextSceneName = "21_AbandonHouse";
    public float delayBeforeLoad = 2f;
    public TextMeshProUGUI infoText;
    public GameObject minigamePanel;

    [Header("Letter UI")]
    public GameObject letterPanel;
    public TextMeshProUGUI letterText;

    private int orderCount = 0;
    private bool isStocked = false; // Đã dọn hàng xong chưa
    private bool isWorking = false; // Đã mở quầy chưa
    private bool isNarrativePlaying = false; // Có đang nói chuyện ma không

    private bool hasApple = false, hasOrange = false;

    void Awake() { Instance = this; }

    void Start()
    {
        infoText.text = "Nhiệm vụ đầu ngày: Dọn 1 thùng Táo và 1 thùng Cam lên 2 quầy!";
    }

    // GỌI KHI ĐẶT THÙNG LÊN BÀN
    public bool ReportDelivery(string itemName)
    {
        if (isStocked) return false;

        if (itemName.Contains("Apple") && !hasApple) hasApple = true;
        else if (itemName.Contains("Orange") && !hasOrange) hasOrange = true;
        else return false; // Không đúng đồ hoặc đã có rồi

        if (hasApple && hasOrange)
        {
            isStocked = true;
            infoText.text = "Hàng đã lên kệ! Ra chỗ CÁI CÂN bấm E để mở quầy.";
        }
        else
        {
            infoText.text = "Đã xong 1 thùng! Còn thiếu 1 thùng nữa.";
        }
        return true;
    }

    // GỌI KHI BẤM E TẠI CÁI CÂN
    public void InteractWithScale()
    {
        if (!isStocked || isNarrativePlaying || minigamePanel.activeSelf) return;

        if (!isWorking)
        {
            isWorking = true;
            ShowNextOrder();
        }
        else if (orderCount < 3)
        {
            // Đã mở quầy thì bấm E sẽ mở Minigame
            OpenMinigame();
        }
    }

    void ShowNextOrder()
    {
        if (orderCount == 0) infoText.text = "[ĐƠN HÀNG 1]: Cần 2kg Táo. (Bấm E để bắt đầu cân)";
        else if (orderCount == 1) infoText.text = "[ĐƠN HÀNG 2]: Cần 1.5kg Cam. (Bấm E để bắt đầu cân)";
        else if (orderCount == 2) infoText.text = "[ĐƠN HÀNG 3]: Lấy lẫn lộn 3kg. (Bấm E để bắt đầu cân)";
    }

    void OpenMinigame() // Chữ g viết thường
    {
        // 1. Đánh thức cái bảng dậy
        minigamePanel.SetActive(true); 

        // 2. Gọi cái hàm OpenMiniGame (chữ G viết hoa) bên file WeighingMiniGame chạy
        minigamePanel.GetComponent<WeighingMiniGame>().OpenMiniGame();
        
        // 3. Hiện text
        infoText.text = "Canh kim vào VÙNG XANH rồi bấm phím SPACE để chốt!";
    }

    // GỌI KHI THẮNG MINIGAME
    public void MinigameSuccess()
    {
        minigamePanel.SetActive(false);
        orderCount++;

        if (orderCount < 3)
        { 
            infoText.text = "Giao thành công! Đang chờ đơn mới...";
            Invoke("ShowNextOrder", 2f);
        }
        else
        {
            StartEndingSequence();
        }
    }

    // CHUỖI HỘI THOẠI KINH DỊ
    void StartEndingSequence()
    {
        isNarrativePlaying = true;
        infoText.text = ""; // Xóa dòng chữ thông báo nhỏ đi

        // Hiện bảng Thư Phản Hồi lên
        letterPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Hiện chuột để bấm nút
        Cursor.visible = true;

        if (letterText != null)
        {
            letterText.text = "KÍNH GỬI CHỦ QUÁN,\n\nĐồ giao tới bị héo quá, không tươi chút nào. Khó chịu thật đấy, nhưng thôi tôi cũng bỏ qua cho lần này...\nTối nay lại phải đi qua khu sau nhà à?\n" +
                        "Ban ngày thì không sao... Nhưng nhớ kỹ lời ta dặn. Nếu đi ngang qua cái giếng cạn, cứ nhìn thẳng mà bước. Đừng có tò mò.\n\n- Khách hàng ẩn danh -";
        }
    }

    // GỌI KHI BẤM NÚT "ĐÓNG THƯ"
    public void CloseLetterAndContinue()
    {
        letterPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Giấu chuột đi lại
        Cursor.visible = false;

        // Bắt đầu gọi ông già nói chuyện
        PlaySpookyVoice();
    }

    void PlaySpookyVoice()
    {
        infoText.text = "Cái giếng đó cạn đã lâu rồi mà nhỉ? hmm..."; // Lời thoại đầu tiên
        Invoke("ProtagonistMonologue", 2f);
    }

    void ProtagonistMonologue()
    {
        infoText.text = " Không lẽ có gì dưới đó sao? Mà thôi cứ nghe theo cho chắc vậy.";
        Invoke("FinishDay", 2f);
    }

    void FinishDay()
    {
        infoText.text = "Hết ca làm việc! Trời cũng sắp tối rồi...";

        Invoke(nameof(LoadNextScene), delayBeforeLoad);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}