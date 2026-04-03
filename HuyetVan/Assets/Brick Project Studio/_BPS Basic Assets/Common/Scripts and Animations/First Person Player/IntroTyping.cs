using System.Collections;
using TMPro;
using UnityEngine;

public class IntroTyping : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    [TextArea(3, 5)]
    public string fullText = "Mình ở căn hộ ngay tầng 1...\nMình nhớ vợ mình hay viết nhật kí...";

    public float typingSpeed = 0.05f;

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
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            textUI.text = fullText;
        }
    }
}