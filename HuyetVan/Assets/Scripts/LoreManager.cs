using UnityEngine;
using System.Collections.Generic;

public class LoreManager : MonoBehaviour
{
    public static LoreManager Instance;
    private HashSet<string> _unlocked = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UnlockEntry(string id)
    {
        if (_unlocked.Contains(id)) return;
        _unlocked.Add(id);
        Debug.Log("Lore mở khóa: " + id);
    }

    public bool IsUnlocked(string id) => _unlocked.Contains(id);
}