using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using CorruptedCircuit.SlidingTilePuzzle.Core;
using CorruptedCircuit.SlidingTilePuzzle.Core.SO;
namespace CorruptedCircuit.SlidingTilePuzzle.Editors
{
    [CustomEditor(typeof(Setup))]
    public class SetupEditor : Editor
    {
        public VisualTreeAsset _Editor;
        private Setup Target => (Setup)target;


        public override VisualElement CreateInspectorGUI()
        {

            var root = _Editor.Instantiate();
            var activateButton = root.Q<Button>();
            bool isActive = GlobalSettings._SetUp == (Setup)target;
            activateButton.text = isActive ? "Activated" : "Activate";
            activateButton.style.backgroundColor = isActive ? Color.darkGreen : Color.darkRed;

            activateButton.clicked += () =>
            {
                GlobalSettings._SetUp = (Setup)target;
                isActive = GlobalSettings._SetUp == (Setup)target;
                activateButton.text = isActive ? "Activated" : "Activate";
                activateButton.style.backgroundColor = isActive ? Color.darkGreen : Color.darkRed;
            };

            var imageContainer = root.Q<VisualElement>("Image");
            var objField = root.Q<ObjectField>();
            var levelList = root.Q<ListView>("LevelList");
            var generalSettingsContainer = root.Q<VisualElement>("Draw");

            var so = new SerializedObject(target);
            QuickDraw(generalSettingsContainer, so.FindProperty(nameof(Setup._GeneralSettings)));

            objField.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue is Sprite s)
                    imageContainer.style.backgroundImage = new StyleBackground(s);
                else
                    imageContainer.style.backgroundImage = null;
            });
            return root;
        }
        private void QuickDraw(VisualElement container, SerializedProperty prop)
        {
            container.Clear();
            var end = prop.GetEndProperty();
            bool enterChildren = true;

            while (prop.NextVisible(enterChildren) && !SerializedProperty.EqualContents(prop, end))
            {
                enterChildren = false;

                var field = new PropertyField();
                field.BindProperty(prop);
                container.Add(field);
            }
        }

    }

}