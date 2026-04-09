using System.Collections.Generic;
using UnityEngine;

namespace CorruptedCircuit.SlidingTilePuzzle.Core.SO
{
    public enum Difficulty { Easy, Medium, Hard }
    [CreateAssetMenu(fileName = "Setup", menuName = "Corrupted Circuit/Sliding Tile Puzzle Setup")]
    public class Setup : ScriptableObject
    {
        public bool _IsActivated;

        public Sprite _Icon;
        public string _Name;
        public string _Catchline;
        public List<Level> _Levels;
        public GeneralSettings _GeneralSettings;
        public int _CurrentLevelNumber => PlayerPrefs.GetInt("SelectedLevel", 0);
        
        // ---- ĐÃ CHỈNH SỬA Ở ĐÂY ----
        // Comment hoặc xóa luôn hàm này để cắt đứt liên kết với GlobalSettings
        /*
        public Setup()
        {
            GlobalSettings._Setups.Add(this);
        }
        */
        // ---------------------------

        public static Setup operator ++(Setup operand)
        {
            int current = PlayerPrefs.GetInt("SelectedLevel", 0);
            PlayerPrefs.SetInt("SelectedLevel", current + 1);
            SaveBool.SetBool("Level " + current.ToString(), true);
            return operand;
        }
        [ContextMenu("Unlock All Levels")]
        public void UnlockAll()
        {
            for (int i = 0; i < _Levels.Count; i++)
            {
                int current = i;
                PlayerPrefs.SetInt("SelectedLevel", current + 1);
                SaveBool.SetBool("Level " + current.ToString(), true);
            }
        }
    }
    
    // ... (Các class Level và GeneralSettings bên dưới giữ nguyên y hệt)
    
    [System.Serializable]
    public class Level
    {
        public List<Texture2D> sourceTextures = new List<Texture2D>();
        public Difficulty Difficulty;
        public Vector2Int gridSize;
        public bool Complete(int i) => SaveBool.GetBool("Level " + i.ToString(), defaultValue: i < 0);
        public bool UnLocked(int i) => Complete(i - 1);
    }
    [System.Serializable]
    public class GeneralSettings
    {
        public Vector2Int padding;
        public float pixelsPerUnit = 100f;

        [Header("UI / Fit")]
        public Vector2 maxParentSize = new Vector2(800, 600);     
        public TileButton tilePrefab;                    
        public bool allowUpscale = true;                 
        public bool clearParentBeforeCreate = true;

        [Header("Timing")]
        public float startDelay = 2f;     
        public float fadeDuration = 1.2f;
        public float fadeInDuration = 1.2f;

        [Header("Movement")]
        public float moveDuration = 0.18f;
        public AnimationCurve moveCurve = default;
        public int shuffleMoves = 80;
        public int perFrame = 5;         
    }
}