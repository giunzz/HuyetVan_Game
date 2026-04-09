using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace CorruptedCircuit.SlidingTilePuzzle.Core
{
    public class TileButton : MonoBehaviour
    {
        public CanvasGroup _CanvasGroup => GetComponent<CanvasGroup>();
        public Vector2Int _CurrentPosition;
        public Vector2Int _CorrectPosition;
        public Action<float> OnProgress;
        public bool _IsEmpty;
        public void InitializeLayers(Sprite[] layerSprites, Vector2Int vector2, UnityAction action = null)
        {
            GetComponent<Button>().onClick.AddListener(action);
            _CorrectPosition = _CurrentPosition = vector2;

            _CanvasGroup.alpha = 1f;
            _CanvasGroup.interactable = true;
            _CanvasGroup.blocksRaycasts = true;

            OnProgress = null;
            var parentRect = GetComponent<RectTransform>();
            int totalLayers = layerSprites.Length;



            for (int i = 0; i < totalLayers; i++)
            {
                var sprite = layerSprites[i];
                if (sprite == null) continue;

                // Create layer object
                var layer = new GameObject($"Layer_{i}", typeof(RectTransform));
                layer.transform.SetParent(transform, false);

                var rect = layer.GetComponent<RectTransform>();
                rect.anchorMin = rect.sizeDelta = rect.anchoredPosition = Vector2.zero;
                rect.anchorMax = Vector2.one;
;

                var image = layer.AddComponent<Image>();
                image.sprite = sprite;

                if (i == 0) continue; // top-most layer stays fully visible

                int index = i; // capture for closure
                OnProgress += value =>
                {
                    float step = 100f / (totalLayers - 1);
                    float start = (index - 1) * step;
                    float end = index * step;
                    float alpha = Mathf.InverseLerp(start, end, value);

                    var color = image.color;
                    color.a = alpha;
                    image.color = color;
                };
            }
            OnProgress?.Invoke(0);
        }


    }
}