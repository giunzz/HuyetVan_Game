using System.Collections;
using TMPro;
using UnityEngine;

public class IntroTyping : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    [TextArea(3, 5)]
    public string fullText = "Mình ở căn hộ ngay tầng 1...\nMình nhớ vợ mình hay viết nhật kí...";

    public float typingSpeed = 0.05f;
    public float stayDuration = 3f;

    private bool isTyping = true;

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textUI.text = "";

        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        yield return new WaitForSeconds(stayDuration);
        HideIntro();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines(); // dừng typing
                textUI.text = fullText;
                isTyping = false;

                StartCoroutine(HideAfterDelay());
            }
        }
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(stayDuration);
        HideIntro();
    }

    void HideIntro()
    {
        Debug.Log("HIDE INTRO"); // 👈 check có chạy không
        gameObject.SetActive(false);
    }
}