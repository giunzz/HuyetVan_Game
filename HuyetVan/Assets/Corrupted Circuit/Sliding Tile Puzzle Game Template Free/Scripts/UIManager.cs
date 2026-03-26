using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using CorruptedCircuit.SlidingTilePuzzle.Core.SO;
namespace CorruptedCircuit.SlidingTilePuzzle.Core
{
    public class UIManager : MonoBehaviour
    {
        public string CreatorID = "Made by Corrupted Circuit";
        public VisualTreeAsset itemTemplate;
        private void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Label>("CreatorID").text = CreatorID;
            root.Q<Label>("Name").text = GlobalSettings._SetUp._Name;
            root.Q<Label>("Catchline").text = GlobalSettings._SetUp._Catchline;
            root.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(GlobalSettings._SetUp._Icon);

            ScrollView scrollView = root.Q<ScrollView>();

            for (int i = 0; i < GlobalSettings._SetUp._Levels.Count; i++)
            {
                int index = i; // capture a local copy
                scrollView.Add(LevelButton(index, GlobalSettings._SetUp._Levels[index], () =>
                {
                    PlayerPrefs.SetInt("SelectedLevel", index);
                    SceneManager.LoadScene("Levels");
                }));
            }

        }
        Button LevelButton(int levelNumber, Level levelSO, Action action = null)
        {
            Button button = itemTemplate.Instantiate().Q<Button>();
            button.Q<Label>("Title").text = "Level " + (levelNumber + 1).ToString();
            Label difficultyLabel = button.Q<Label>("Difficulty");
            difficultyLabel.text = levelSO.Difficulty.ToString();
            button.Q<Label>("GridSize").text = levelSO.gridSize.x.ToString() + "x" + levelSO.gridSize.y.ToString() + " grid";

            difficultyLabel.RemoveFromClassList("difficulty-easy");
            difficultyLabel.RemoveFromClassList("difficulty-medium");
            difficultyLabel.RemoveFromClassList("difficulty-hard");

            switch (levelSO.Difficulty)
            {
                case Difficulty.Easy: difficultyLabel.AddToClassList("difficulty-easy"); break;
                case Difficulty.Medium: difficultyLabel.AddToClassList("difficulty-medium"); break;
                case Difficulty.Hard: difficultyLabel.AddToClassList("difficulty-hard"); break;
            }
            bool unlocked = levelSO.UnLocked(levelNumber);

            button.SetEnabled(unlocked);
            button.style.opacity = unlocked ? 1f : 0.4f;
            button.clicked += () => { if (unlocked) { action?.Invoke(); } };
            return button;
        }
    }
}