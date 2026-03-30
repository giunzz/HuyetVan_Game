using UnityEngine;
using System.Collections;

public class Microscope : MonoBehaviour
{
    public static Microscope Instance;

    public GameObject microscopeUI;
    private bool canUse = false;

    void Awake()
    {
        Instance = this;
    }

    public void EnableMicroscope()
    {
        canUse = true;
        Debug.Log("🔬 Microscope unlocked");
    }

    public void Interact()
    {
        if (InventoryManager.Instance == null ||
            !InventoryManager.Instance.HasQTipSample())
        {
            Debug.Log("❌ QTip chưa có mẫu!");
            return;
        }

        Debug.Log("🔬 Đang xét nghiệm mẫu...");

        // bật UI
        if (microscopeUI != null)
            microscopeUI.SetActive(true);
    }

    IEnumerator Analyze()
    {
        Debug.Log("⏳ Đang phân tích mẫu...");

        yield return new WaitForSeconds(2f);

        Debug.Log("✅ Phân tích xong!");

        // 👉 GỌI TIẾP FLOW GAME
        if (Autopsy2DMiniGame.Instance != null)
        {
            Autopsy2DMiniGame.Instance.OnPuzzleSolved();
        }
    }
}