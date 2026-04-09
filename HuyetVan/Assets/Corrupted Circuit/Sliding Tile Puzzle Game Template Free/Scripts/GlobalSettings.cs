using UnityEngine;
using CorruptedCircuit.SlidingTilePuzzle.Core.SO;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace CorruptedCircuit.SlidingTilePuzzle.Core
{
    public class ID
    {
       public string AssetID;
    }
    public static class GlobalSettings
    {
        public static List<Setup> _Setups = new List<Setup>();
        public static Setup _SetUp { get => LoadAsset<Setup>(_SetUpID); set => SetAssetPath(_SetUpID, value); }
        public const string _SetUpID = "config";

        public static int _CurrentLevelNumber => _SetUp._CurrentLevelNumber;
        public static Level _CurrentLevel => _SetUp._Levels[_CurrentLevelNumber];

        public static void ActivateSetUp(Setup setup)
        {
            for (int i = 0; i < _Setups.Count; i++)
            {
                _Setups[i]._IsActivated = _Setups.IndexOf(setup) == i;
            }
        }

        public static void SetAssetPath<T>(string id, T settingsAsset) where T : ScriptableObject
        {
            ID asset = new ID() { AssetID = settingsAsset.name };

            string jsonString = JsonUtility.ToJson(asset, true);

            // Make sure Resources folder exists
            string resourcesPath = Path.Combine("Assets/Corrupted Circuit/Sliding Tile Puzzle Game Template Free", "Resources");
            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            // Full path to file
            string filePath = Path.Combine(resourcesPath, _SetUpID + ".json");

            // Write JSON file
            File.WriteAllText(filePath, jsonString);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        public static T LoadAsset<T>(string id) where T : ScriptableObject
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>(_SetUpID);
            if (jsonTextAsset != null)
            {
                var ID = JsonUtility.FromJson<ID>(jsonTextAsset.text);
                var asset = Resources.Load<T>(ID.AssetID);
                if (asset)
                {
                    return asset;
                }
                else
                {
                    Debug.LogError("Asset not found");
                }

            }
            else
            {
                Debug.LogError("JSON file not found in Resources!");
            }
            return default;
        }

        static VisualElement root;
        static Button Next => root.Q<Button>("Next");
        static Button Reset => root.Q<Button>("Reset");
        static Button Back => root.Q<Button>("Back");
        public static void DisplayLevelDetails(VisualElement visualElement)
        {
            root = visualElement;
            Next.style.display = DisplayStyle.None;
            Reset.style.display = DisplayStyle.Flex;

            void reload()
            {
                Scene current = SceneManager.GetActiveScene();
                SceneManager.LoadScene(current.name);
            }
            Next.clicked += reload;
            Reset.clicked += reload;
            Back.clicked += () => SceneManager.LoadScene("Main Menu");
            root.Q<Label>("Title").text = "Level " + (_CurrentLevelNumber + 1).ToString();
            root.Q<Label>("GridSize").text = _CurrentLevel.gridSize.x.ToString() + "x" + _CurrentLevel.gridSize.y.ToString();

        }
        public static void OnSolved()
        {
            if (_CurrentLevelNumber >= (_SetUp._Levels.Count - 1))
            {
                return;
            }
            _SetUp++;
            Next.style.display = DisplayStyle.Flex;
            Reset.style.display = DisplayStyle.None;
        }

        public static void SetAnchorCenterKeepSize(this RectTransform rt)
        {
            if (rt == null) return;
            var parent = rt.parent as RectTransform;
            if (parent == null) return;

            Vector3[] worldCorners = new Vector3[4];
            rt.GetWorldCorners(worldCorners);
            Vector3 worldCenter = (worldCorners[0] + worldCorners[2]) * 0.5f;
            Vector2 currentSize = rt.rect.size;

            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);

            Vector3 localCenter3 = parent.InverseTransformPoint(worldCenter);

            rt.anchoredPosition = new Vector2(localCenter3.x, localCenter3.y);
            rt.sizeDelta = currentSize;
        }
    }
    public static class SaveBool
    {
        public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
        public static bool GetBool(string key, bool defaultValue = false)
        {
            int defaultInt = defaultValue ? 1 : 0;
            int stored = PlayerPrefs.GetInt(key, defaultInt);
            return stored == 1;
        }
        public static void SetBool(this PlayerPrefs playerPrefs, string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

        public static bool GetBool(this PlayerPrefs playerPrefs)
        {
            return true;
        }
    }

}