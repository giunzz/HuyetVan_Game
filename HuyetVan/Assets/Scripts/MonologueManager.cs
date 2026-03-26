using UnityEngine;
using System.Collections;
using TMPro;

public class MonologueManager : MonoBehaviour
{
    public static MonologueManager Instance;

    [Header("UI")]
    public TextMeshProUGUI monologueText;
    public float displayTime = 3f; // giây hiện chữ

    private Coroutine _current;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (monologueText != null)
            monologueText.text = "";
    }

    public void Show(string text)
    {
        if (_current != null)
            StopCoroutine(_current);
        _current = StartCoroutine(ShowRoutine(text));
    }

    IEnumerator ShowRoutine(string text)
    {
        monologueText.text = text;
        yield return new WaitForSeconds(displayTime);
        monologueText.text = "";
    }
}