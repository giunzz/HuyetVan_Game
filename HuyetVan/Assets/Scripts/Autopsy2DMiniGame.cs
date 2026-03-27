using UnityEngine;
using UnityEngine.Events;
using CorruptedCircuit.SlidingTilePuzzle.Core;

public class Autopsy2DMiniGame : MonoBehaviour
{
    public static Autopsy2DMiniGame Instance;

    [Header("Tham chiếu")]
    public GameObject puzzleCanvas;
    public SlidingTilePuzzleManager puzzleManager;

    [Header("Events")]
    public UnityEvent onSolved;
    public UnityEvent onFailed;

    private bool _listening = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenMiniGame()
    {
        if (puzzleCanvas == null || puzzleManager == null)
        {
            Debug.LogError("Chưa gán puzzleCanvas hoặc puzzleManager!");
            return;
        }
        puzzleCanvas.SetActive(true);
        if (!_listening)
        {
            puzzleManager.OnSolved.AddListener(OnPuzzleSolved);
            _listening = true;
        }
    }

    void OnPuzzleSolved()
    {
        CloseMiniGame();
        if (LoreManager.Instance != null)
            LoreManager.Instance.UnlockEntry("corpse_01");
        onSolved?.Invoke();
    }

    public void OnPuzzleFailed()
    {
        CloseMiniGame();
        onFailed?.Invoke();
    }

    void CloseMiniGame()
    {
        puzzleCanvas.SetActive(false);
        puzzleManager.OnSolved.RemoveListener(OnPuzzleSolved);
        _listening = false;
    }
}