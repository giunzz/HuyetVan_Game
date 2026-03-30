using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI")]
    public GameObject scalpelIconUI;
    public GameObject qtipIconUI;

    [Header("Viewmodel (cầm trên tay)")]
    public GameObject scalpelInHand;
    public GameObject qtipInHand;

    // ===== STATE =====
    private bool _hasScalpel = false;
    private bool _hasQTip = false;
    private bool _qtipHasSample = false;

    private bool _isScalpelEquipped = false;
    private bool _isQTipEquipped = false;

    // cache renderer để tránh gọi nhiều lần
    private Renderer _qtipRenderer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Tắt UI ban đầu
        if (scalpelIconUI != null) scalpelIconUI.SetActive(false);
        if (qtipIconUI != null) qtipIconUI.SetActive(false);

        // Tắt item trên tay
        if (scalpelInHand != null) scalpelInHand.SetActive(false);
        if (qtipInHand != null) qtipInHand.SetActive(false);

        // cache renderer QTip
        if (qtipInHand != null)
        {
            _qtipRenderer = qtipInHand.GetComponentInChildren<Renderer>();
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            EquipScalpel();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            EquipQTip();
        }
    }

    // ================= ADD ITEM =================
    public void AddScalpelToBag()
    {
        if (_hasScalpel) return;

        _hasScalpel = true;

        if (scalpelIconUI != null)
            scalpelIconUI.SetActive(true);

        Debug.Log("✅ Đã nhặt Scalpel");
    }

    public void AddQTipToBag()
    {
        if (_hasQTip) return;

        _hasQTip = true;

        if (qtipIconUI != null)
            qtipIconUI.SetActive(true);

        Debug.Log("✅ Đã nhặt QTip");
    }

    // ================= SAMPLE =================
    public void SetQTipHasSample()
    {
        _qtipHasSample = true;

        if (_qtipRenderer != null)
        {
            // hỗ trợ cả URP & Standard shader
            if (_qtipRenderer.material.HasProperty("_BaseColor"))
                _qtipRenderer.material.SetColor("_BaseColor", Color.black);
            else
                _qtipRenderer.material.color = Color.black;
        }

        Debug.Log("🧪 QTip đã có mẫu (đen)");
    }

    public bool HasQTipSample()
    {
        return _qtipHasSample;
    }

    // ================= EQUIP =================
    void EquipScalpel()
    {
        if (!_hasScalpel)
        {
            Debug.Log("❌ Không có Scalpel");
            return;
        }

        ResetHand();

        _isScalpelEquipped = true;

        if (scalpelInHand != null)
            scalpelInHand.SetActive(true);

        Debug.Log("✋ Cầm Scalpel");
    }

    void EquipQTip()
    {
        if (!_hasQTip)
        {
            Debug.Log("❌ Không có QTip");
            return;
        }

        ResetHand();

        _isQTipEquipped = true;

        if (qtipInHand != null)
            qtipInHand.SetActive(true);

        Debug.Log("✋ Cầm QTip");

        // nếu QTip đã có sample → đảm bảo vẫn màu đen khi equip lại
        if (_qtipHasSample)
        {
            SetQTipHasSample();
        }
    }

    void ResetHand()
    {
        _isScalpelEquipped = false;
        _isQTipEquipped = false;

        if (scalpelInHand != null)
            scalpelInHand.SetActive(false);

        if (qtipInHand != null)
            qtipInHand.SetActive(false);
    }

    // ================= CHECK =================
    public bool IsScalpelEquipped()
    {
        return _isScalpelEquipped;
    }

    public bool IsQTipEquipped()
    {
        return _isQTipEquipped;
    }
}