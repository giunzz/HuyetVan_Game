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
        if (!canUse)
        {
            Debug.Log("❌ Chưa unlock microscope");
            return;
        }

        if (microscopeUI != null)
            microscopeUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(Analyze());
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