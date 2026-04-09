using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI")]
    public GameObject scalpelIconUI;
    public GameObject qtipIconUI;

    [Header("QTip Visual")]
    public Renderer qtipRenderer;   // mesh renderer của qtip
    public Color cleanColor = Color.white;
    public Color dirtyColor = Color.black;

    [Header("Viewmodel (cầm trên tay)")]
    public GameObject scalpelInHand;
    public GameObject qtipInHand;

    // ===== Trạng thái =====
    private bool hasScalpel = false;
    private bool hasQTip = false;

    private bool isScalpelEquipped = false;
    private bool isQTipEquipped = false;

    private bool qtipHasSample = false;

    // ================= INIT =================
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // tránh crash nếu chưa gán
        if (scalpelInHand != null) scalpelInHand.SetActive(false);
        if (qtipInHand != null) qtipInHand.SetActive(false);

        if (scalpelIconUI != null) scalpelIconUI.SetActive(false);
        if (qtipIconUI != null) qtipIconUI.SetActive(false);

        ResetQTipColor();
    }

    // ================= INPUT =================
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleScalpel();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleQTip();
        }
    }

    // ================= PICKUP =================
    public void AddScalpelToBag()
    {
        hasScalpel = true;

        if (scalpelIconUI != null)
            scalpelIconUI.SetActive(true);

        Debug.Log("🗡️ Đã nhặt Scalpel");
    }

    public void AddQTipToBag()
    {
        hasQTip = true;

        if (qtipIconUI != null)
            qtipIconUI.SetActive(true);

        ResetQTipColor();

        Debug.Log("🧪 Đã nhặt Q-Tip");
    }

    // ================= EQUIP =================
    void ToggleScalpel()
    {
        if (!hasScalpel)
        {
            Debug.Log("❌ Chưa có Scalpel");
            return;
        }

        isScalpelEquipped = !isScalpelEquipped;

        if (scalpelInHand != null)
            scalpelInHand.SetActive(isScalpelEquipped);

        if (isScalpelEquipped)
        {
            isQTipEquipped = false;
            if (qtipInHand != null) qtipInHand.SetActive(false);

            Debug.Log("🗡️ Cầm Scalpel");
        }
        else
        {
            Debug.Log("📦 Cất Scalpel");
        }
    }

    void ToggleQTip()
    {
        if (!hasQTip)
        {
            Debug.Log("❌ Chưa có QTip");
            return;
        }

        isQTipEquipped = !isQTipEquipped;

        if (qtipInHand != null)
            qtipInHand.SetActive(isQTipEquipped);

        if (isQTipEquipped)
        {
            isScalpelEquipped = false;
            if (scalpelInHand != null) scalpelInHand.SetActive(false);

            Debug.Log("🧪 Cầm QTip");
        }
        else
        {
            Debug.Log("📦 Cất QTip");
        }
    }

    // ================= STATE =================
    public bool IsScalpelEquipped()
    {
        return isScalpelEquipped;
    }

    public bool IsQTipEquipped()
    {
        return isQTipEquipped;
    }

    public bool HasSample()
    {
        return qtipHasSample;
    }

    // ================= SAMPLE =================
    public void SetQTipHasSample()
    {
        qtipHasSample = true;

        Debug.Log("🧫 QTip đã có mẫu");

        ApplyQTipColor(dirtyColor);
    }

    // ================= COLOR =================
    void ResetQTipColor()
    {
        ApplyQTipColor(cleanColor);
    }

    void ApplyQTipColor(Color color)
    {
        if (qtipRenderer == null) return;

        // hỗ trợ cả URP và Standard
        if (qtipRenderer.material.HasProperty("_BaseColor"))
        {
            qtipRenderer.material.SetColor("_BaseColor", color);
        }
        else
        {
            qtipRenderer.material.color = color;
        }
    }

    // ================= RESET =================
    public void ClearAll()
    {
        hasScalpel = false;
        hasQTip = false;

        isScalpelEquipped = false;
        isQTipEquipped = false;

        qtipHasSample = false;

        if (scalpelInHand != null) scalpelInHand.SetActive(false);
        if (qtipInHand != null) qtipInHand.SetActive(false);

        if (scalpelIconUI != null) scalpelIconUI.SetActive(false);
        if (qtipIconUI != null) qtipIconUI.SetActive(false);

        ResetQTipColor();
    }
}