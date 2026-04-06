using System.Collections;
using UnityEngine;
using TMPro;

public class ApartmentPuzzle : MonoBehaviour
{
    [Header("UI")]
    public GameObject puzzleCanvas;
    public GameObject diaryPanel;

    [Header("Text")]
    public TextMeshProUGUI textUI;

    [TextArea(3, 10)]
    public string text1 = @"Các đoạn trước của nhật ký đã rách mất...

    Ngày 15 tháng 2 (Dương lịch): Bé An sốt không hạ. Thuốc của anh không có tác dụng, máy móc của bệnh viện cũng không tìm ra bệnh. Nhìn con bé thoi thóp, trái tim tôi như có nghìn mũi kim đâm vào. Anh là bác sĩ pháp y, anh chỉ giỏi nhìn xác chết, còn tôi cần một sự sống! Tôi đã lén bế con đến gặp Thầy Bá. Thầy bảo... con bé không bệnh. Nó đang phải ""trả nợ"" thay cho cái nghiệp mà dòng họ tôi đã vay ở dưới quê.";

    [TextArea(3, 10)]
    public string text2 = @"Ngày 20 tháng 2: Anh lại cằn nhằn về việc tôi tin vào bùa chú. Anh nói khoa học sẽ cứu con. Nhưng khoa học của anh có thấy được cái bóng đen đứng ở góc phòng mỗi đêm không? Có nghe thấy tiếng ""Huyết Nghãi"" nhai xương trong giấc mơ không? Thầy Bá đã cho tôi một lá bùa để cầm cự cho An, nhưng nó chỉ là giải pháp tạm thời.";

    [TextArea(3, 10)]
    public string text3 = @"Ngày 28 tháng 2: Tôi không thể chờ đợi...

Gửi anh...

Nếu anh đọc được những dòng này...

Hãy lái xe về phía quốc lộ...
Quán cơm Bà Ba gần cầu Phao...

<color=#ff0000>Làm ơn... hãy cứu lấy con chúng ta.</color>";

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;
    public float delayBetweenTexts = 1.5f;

    [Header("Player")]
    public MonoBehaviour playerMovement;

    public float delayOpenDiary = 2f;

    private bool isSolved = false;
    private bool skipTyping = false;

    // 🔓 OPEN PUZZLE
    public void OpenPuzzle()
    {
        puzzleCanvas.SetActive(true);
        diaryPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    // ✅ SOLVE
    public void OnPuzzleSolved()
    {
        if (isSolved) return;
        isSolved = true;

        puzzleCanvas.SetActive(false);

        StartCoroutine(OpenDiaryFlow());
    }

    IEnumerator OpenDiaryFlow()
    {
        yield return new WaitForSeconds(delayOpenDiary);

        diaryPanel.SetActive(true);

        yield return StartCoroutine(TypeText(text1));
        yield return WaitOrClick();

        yield return StartCoroutine(TypeText(text2));
        yield return WaitOrClick();

        yield return StartCoroutine(TypeText(text3));
    }

    // 🧠 Typing + skip
    IEnumerator TypeText(string content)
    {
        textUI.text = "";
        skipTyping = false;

        foreach (char c in content)
        {
            // nếu click → hiện full luôn
            if (skipTyping)
            {
                textUI.text = content;
                yield break;
            }

            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // ⏳ đợi hoặc click để next
    IEnumerator WaitOrClick()
    {
        float timer = 0f;

        while (timer < delayBetweenTexts)
        {
            if (Input.GetMouseButtonDown(0))
                yield break;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    void Update()
    {
        // click để skip typing
        if (Input.GetMouseButtonDown(0))
        {
            skipTyping = true;
        }
    }

    public void CloseDiary()
    {
        diaryPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}