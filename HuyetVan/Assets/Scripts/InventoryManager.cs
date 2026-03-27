using UnityEngine;
using UnityEngine.UI; // Thư viện dùng cho UI cũ
using UnityEngine.InputSystem; // BẮT BUỘC THÊM: Thư viện cho hệ thống Input mới

public class InventoryManager : MonoBehaviour
{
    // Singleton giúp gọi túi đồ từ mọi nơi
    public static InventoryManager Instance;

    [Header("UI Túi Đồ")]
    public GameObject scalpelIconUI; // Hình ảnh con dao trong ô Slot1

    [Header("Vật dụng trên tay (Viewmodel)")]
    public GameObject scalpelInHand; // Con dao gắn ở Camera bạn làm ở Bước 2

    // Trạng thái túi đồ
    private bool _hasScalpelInBag = false;
    private bool _isScalpelEquipped = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Mới vào game: chưa có dao trong túi, chưa cầm trên tay
        if (scalpelIconUI != null) scalpelIconUI.SetActive(false);
        if (scalpelInHand != null) scalpelInHand.SetActive(false);
    }

    void Update()
    {
        // Kiểm tra xem bàn phím có đang hoạt động không để tránh lỗi vặt
        if (Keyboard.current == null) return;

        // Bấm phím 1 để lấy/cất dao (Đã sửa thành chuẩn New Input System)
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            ToggleScalpel();
        }
    }

    // Hàm được gọi từ InteractManager khi nhặt dao mổ
    public void AddScalpelToBag()
    {
        if (_hasScalpelInBag) return; // Đã có rồi thì thôi

        _hasScalpelInBag = true;

        // Hiện icon con dao trong túi đồ UI
        if (scalpelIconUI != null) scalpelIconUI.SetActive(true);

        Debug.Log("Đã bỏ dao mổ vào túi đồ.");
        
        // Bạn có thể cho độc thoại nội tâm ở đây:
        if (MonologueManager.Instance != null)
            MonologueManager.Instance.Show("Dao mổ đây rồi. Bỏ vào túi thôi.");
    }

    // Xử lý logic lấy/cất dao khi bấm phím 1
    void ToggleScalpel()
    {
        if (!_hasScalpelInBag)
        {
            Debug.Log("Trong túi không có dao mổ.");
            return;
        }

        // Đổi trạng thái cầm/cất
        _isScalpelEquipped = !_isScalpelEquipped;

        // Hiện/Ẩn mô hình dao trên tay nhân vật
        if (scalpelInHand != null)
        {
            scalpelInHand.SetActive(_isScalpelEquipped);
            Debug.Log(_isScalpelEquipped ? "✋ Đã cầm dao mổ trên tay." : "🎒 Đã cất dao mổ vào túi.");
        }
    }

    public bool IsScalpelEquipped()
    {
        return _isScalpelEquipped;
    }
}