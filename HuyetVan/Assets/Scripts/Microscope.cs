using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Microscope : MonoBehaviour
{
    public static Microscope Instance;

    [Header("UI")]
    public GameObject microscopeUI;
    public Slider focusSlider;

    [Header("Visual")]
    public GameObject blurryImage;
    public GameObject clearImage;

    [Header("Text")]
    public TextMeshProUGUI microscopeText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip creepySound;

    [Header("Scene")]
    public string nextSceneName = "01_apartment";

    private bool hasCompleted = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (microscopeUI != null)
            microscopeUI.SetActive(false);

        if (microscopeText != null)
            microscopeText.text = "";
    }

    public void Interact()
    {
        Debug.Log("===== INTERACT =====");

        if (InventoryManager.Instance == null ||
            !InventoryManager.Instance.HasSample())
        {
            Debug.Log("❌ Chưa có sample");
            return;
        }

        // bật UI
        microscopeUI.SetActive(true);

        // mở chuột
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // reset slider
        focusSlider.value = 0;

        hasCompleted = false;

        // reset hình
        blurryImage.SetActive(true);
        clearImage.SetActive(false);

        // text đầu
        ShowText("Tế bào chết... Dấu vết của xyanua? Hay kim loại nặng?");

        // slider event
        focusSlider.onValueChanged.RemoveAllListeners();
        focusSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        Debug.Log("🎚 Slider value: " + value);

        if (value >= 1f && !hasCompleted)
        {
            hasCompleted = true;
            StartCoroutine(RevealFlow());
        }
    }

    IEnumerator RevealFlow()
    {
        Debug.Log("===== REVEAL FLOW =====");

        // 👉 đổi hình
        blurryImage.SetActive(false);
        clearImage.SetActive(true);

        // 👉 âm thanh
        if (audioSource != null && creepySound != null)
        {
            audioSource.PlayOneShot(creepySound);
        }

        // 👉 TEXT 1
        ShowText("Cái... thứ quỷ gì thế này?! Ký sinh trùng? Không... Cấu trúc này không thuộc về sinh học!");

        yield return new WaitForSeconds(2f);

        // 👉 TEXT 2
        ShowText(" Căn nguyên không nằm ở cái xác. Phải về nhà. Chắc chắn vợ mình đã để lại thứ gì đó trước khi bỏ đi.");

        yield return new WaitForSeconds(3f);

        // 👉 tắt UI
        microscopeUI.SetActive(false);

        // 👉 khóa lại chuột
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 👉 load scene
        SceneManager.LoadScene(nextSceneName);
    }

    void ShowText(string text)
    {
        if (microscopeText != null)
        {
            microscopeText.text = text;
            Debug.Log("📝 " + text);
        }
        else
        {
            Debug.LogError("❌ TEXT NULL");
        }
    }
}