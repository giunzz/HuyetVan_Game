using CorruptedCircuit.SlidingTilePuzzle.Core.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CorruptedCircuit.SlidingTilePuzzle.Core
{
    public class SlidingTilePuzzleManager : MonoBehaviour
    {
        [Header("UI / Fit")]
        public RectTransform puzzleParent;

        GeneralSettings _gen;
        Level _lvl;
        TileButton[,] _grid;
        CanvasGroup _parentCg;
        Vector2 _parentSize;
        float _scaleX, _scaleY;
        bool _busy, _inputEnabled, _allowCompletionDebug;
        int _correctCount;
        float _prevPct = -1f;
        public Action<float> Progress;
        public UnityEvent<float> OnProgress;
        public UnityEvent OnStart, OnSolved, OnComplete;
        int TotalTiles => Mathf.Max(1, _lvl.gridSize.x * _lvl.gridSize.y - 1);
        void Start()
        {
            _gen = GlobalSettings._SetUp._GeneralSettings;
            _lvl = GlobalSettings._CurrentLevel;
            GlobalSettings.DisplayLevelDetails(GetComponent<UnityEngine.UIElements.UIDocument>().rootVisualElement);
            if (_lvl == null)
            {
                Debug.LogError("SlidingTilePuzzleManager: Setup is not assigned.");
                return;
            }
            if (_lvl.gridSize.x < 2 || _lvl.gridSize.y < 2)
            {
                Debug.LogError("SlidingTilePuzzleManager: level.gridSize must be >=2.");
                return;
            }
       
            puzzleParent.SetAnchorCenterKeepSize();
            FitParent();
            StartCoroutine(StartupRoutine());
            OnSolved.AddListener(GlobalSettings.OnSolved);
            Progress += i => OnProgress?.Invoke(i);
        }

        void FitParent()
        {
            Vector2 allowed = (puzzleParent.sizeDelta.sqrMagnitude > 0.0001f)
                ? puzzleParent.sizeDelta
                : _gen.maxParentSize;

            if (allowed.x <= 0 || allowed.y <= 0)
                allowed = new Vector2(_lvl.sourceTextures[0].width, _lvl.sourceTextures[0].height);

            float s = Mathf.Min(allowed.x / _lvl.sourceTextures[0].width, allowed.y / _lvl.sourceTextures[0].height);
            if (!_gen.allowUpscale) s = Mathf.Min(s, 1f);
            if (!float.IsFinite(s) || s <= 0f) s = 1f;

            _parentSize = new Vector2(_lvl.sourceTextures[0].width * s, _lvl.sourceTextures[0].height * s);
            puzzleParent.anchorMin = puzzleParent.anchorMax = puzzleParent.pivot = new Vector2(0.5f, 0.5f);
            puzzleParent.sizeDelta = _parentSize;

            _scaleX = _parentSize.x / _lvl.sourceTextures[0].width;
            _scaleY = _parentSize.y / _lvl.sourceTextures[0].height;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleParent);
        }

        IEnumerator StartupRoutine()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleParent);
            yield return null;

            _parentCg = puzzleParent.GetComponent<CanvasGroup>() ?? puzzleParent.gameObject.AddComponent<CanvasGroup>();
            _parentCg.interactable = false;
            _parentCg.blocksRaycasts = true;
            _inputEnabled = false;

            float t0 = Time.realtimeSinceStartup;
            yield return StartCoroutine(BuildTiles(_gen.perFrame));
            float buildTime = Time.realtimeSinceStartup - t0;

            float adj = Mathf.Max(0f, _gen.startDelay - buildTime);
            SetAllVisible(true);
            if (adj > 0f) yield return new WaitForSeconds(adj);

            var empty = FindEmpty();
            if (empty != null && empty._CanvasGroup != null)
            {
                float time = 0f, startAlpha = empty._CanvasGroup.alpha;
                while (time < _gen.fadeDuration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / _gen.fadeDuration);
                    empty._CanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
                    yield return null;
                }
                empty._CanvasGroup.alpha = 0f;
                empty._CanvasGroup.interactable = false;
                empty._CanvasGroup.blocksRaycasts = false;
            }

            yield return StartCoroutine(ShuffleRoutine(_gen.shuffleMoves == 0 ? _lvl.gridSize.x * _lvl.gridSize.y * 10 : _gen.shuffleMoves));
            _allowCompletionDebug = true;
            UpdateProgress();

            _inputEnabled = true;
            _parentCg.interactable = true;
            _parentCg.blocksRaycasts = true;
            OnStart?.Invoke();
        }

        IEnumerator BuildTiles(int perFrame)
        {
            if (_gen.clearParentBeforeCreate)
            {
                for (int i = puzzleParent.childCount - 1; i >= 0; i--)
                    DestroyImmediate(puzzleParent.GetChild(i).gameObject);
            }

            int W = _lvl.sourceTextures[0].width, H = _lvl.sourceTextures[0].height;
            int availW = W - _gen.padding.x * (_lvl.gridSize.y - 1);
            int availH = H - _gen.padding.y * (_lvl.gridSize.x - 1);
            int baseW = Mathf.Max(1, availW / _lvl.gridSize.y);
            int baseH = Mathf.Max(1, availH / _lvl.gridSize.x);

            int[] colW = new int[_lvl.gridSize.y];
            for (int c = 0; c < colW.Length; c++) colW[c] = baseW;
            int[] rowH = new int[_lvl.gridSize.x];
            for (int r = 0; r < rowH.Length; r++) rowH[r] = baseH;

            _grid = new TileButton[_lvl.gridSize.y, _lvl.gridSize.x];
            int created = 0;
            int yTop = H;

            for (int r = 0; r < _lvl.gridSize.x; r++)
            {
                int h = rowH[r];
                yTop -= h;
                int x = 0;
                for (int c = 0; c < _lvl.gridSize.y; c++)
                {
                    int w = colW[c];
                    var rect = new Rect(x, yTop, w, h);
                    var go = Instantiate(_gen.tilePrefab, puzzleParent, false);
                    var rt = go.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(w * _scaleX, h * _scaleY);
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = new Vector2(
                        (rect.x + rect.width * 0.5f - W * 0.5f) * _scaleX,
                        (rect.y + rect.height * 0.5f - H * 0.5f) * _scaleY
                    );

                    var layerSprites = new Sprite[_lvl.sourceTextures.Count];
                    for (int i = 0; i < _lvl.sourceTextures.Count; i++)
                        layerSprites[i] = Sprite.Create(_lvl.sourceTextures[i], rect, new Vector2(0.5f, 0.5f), _gen.pixelsPerUnit);

                    go.InitializeLayers(layerSprites, new Vector2Int(c, r), () => OnTileClick(go));
                    Progress += go.OnProgress;
                    _grid[c, r] = go;

                    x += w + _gen.padding.x;
                    if (++created >= perFrame)
                    {
                        created = 0;
                        yield return null;
                    }
                }
                yTop -= _gen.padding.y;
            }

            var last = _grid[_lvl.gridSize.y - 1, _lvl.gridSize.x - 1];
            last._IsEmpty = true;
        }

        void OnTileClick(TileButton tile)
        {
            if (!_inputEnabled || _busy) return;
            var empty = GetEmptyNeighbor(tile._CurrentPosition);
            if (empty != null) StartCoroutine(SwapRoutine(tile, empty));
        }

        IEnumerator SwapRoutine(TileButton tile, TileButton empty)
        {
            _busy = true;
            var tr = tile.GetComponent<RectTransform>();
            var er = empty.GetComponent<RectTransform>();
            var a = tr.anchoredPosition;
            var b = er.anchoredPosition;
            float t = 0f;
            while (t < _gen.moveDuration)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / _gen.moveDuration);
                tr.anchoredPosition = Vector2.LerpUnclamped(a, b, _gen.moveCurve.Evaluate(k));
                yield return null;
            }
            tr.anchoredPosition = b;
            er.anchoredPosition = a;
            var tp = tile._CurrentPosition; var ep = empty._CurrentPosition;
            _grid[tp.x, tp.y] = empty;
            _grid[ep.x, ep.y] = tile;
            empty._CurrentPosition = tp; tile._CurrentPosition = ep;

            if (empty._IsEmpty)
            {
                empty._CanvasGroup.alpha = 0f;
                empty._CanvasGroup.interactable = false;
                empty._CanvasGroup.blocksRaycasts = false;
                tile._CanvasGroup.alpha = 1f;
                tile._CanvasGroup.interactable = true;
                tile._CanvasGroup.blocksRaycasts = true;
            }

            _busy = false;
            if (_allowCompletionDebug) UpdateProgress();
            if (IsSolved()) StartCoroutine(CompleteRoutine());
        }

        IEnumerator ShuffleRoutine(int moves)
        {
            var rnd = new System.Random();
            for (int i = 0; i < moves; i++)
            {
                var e = FindEmpty();
                var neigh = GetNeighbors(e._CurrentPosition);
                if (neigh.Count > 0)
                {
                    var pick = neigh[rnd.Next(neigh.Count)];
                    yield return SwapNoAnim(pick, e);
                }
            }
        }

        IEnumerator SwapNoAnim(TileButton tile, TileButton empty)
        {
            var a = tile.GetComponent<RectTransform>().anchoredPosition;
            var b = empty.GetComponent<RectTransform>().anchoredPosition;
            tile.GetComponent<RectTransform>().anchoredPosition = b;
            empty.GetComponent<RectTransform>().anchoredPosition = a;
            var tp = tile._CurrentPosition; var ep = empty._CurrentPosition;
            _grid[tp.x, tp.y] = empty;
            _grid[ep.x, ep.y] = tile;
            empty._CurrentPosition = tp; tile._CurrentPosition = ep;
            yield return null;
        }

        bool IsSolved()
        {
            foreach (var t in _grid)
                if (t._CurrentPosition != t._CorrectPosition) return false;
            return true;
        }

        void UpdateProgress()
        {
            _correctCount = 0;
            foreach (var t in _grid)
                if (!t._IsEmpty && t._CurrentPosition == t._CorrectPosition) _correctCount++;
            float pct = TotalTiles > 0 ? (100f * _correctCount / TotalTiles) : 0f;
            if (!Mathf.Approximately(pct, _prevPct))
            {
                _prevPct = pct;
                Progress?.Invoke(pct);
            }
        }

        List<TileButton> GetNeighbors(Vector2Int p)
        {
            var list = new List<TileButton>(4);
            var dirs = new[] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
            for (int i = 0; i < dirs.Length; i++)
            {
                var n = p + dirs[i];
                if (n.x >= 0 && n.x < _lvl.gridSize.y && n.y >= 0 && n.y < _lvl.gridSize.x)
                    list.Add(_grid[n.x, n.y]);
            }
            return list;
        }

        TileButton FindEmpty()
        {
            foreach (var t in _grid)
                if (t._IsEmpty) return t;
            return null;
        }

        TileButton GetEmptyNeighbor(Vector2Int p)
        {
            var dirs = new[] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
            for (int i = 0; i < dirs.Length; i++)
            {
                var n = p + dirs[i];
                if (n.x >= 0 && n.x < _lvl.gridSize.y && n.y >= 0 && n.y < _lvl.gridSize.x)
                {
                    var t = _grid[n.x, n.y];
                    if (t._IsEmpty) return t;
                }
            }
            return null;
        }

        IEnumerator CompleteRoutine()
        {
            _inputEnabled = false;
            _parentCg.interactable = false;
            _parentCg.blocksRaycasts = true;
            OnSolved?.Invoke();

            var empty = FindEmpty();
            if (empty != null && empty._CanvasGroup != null)
            {
                empty._CanvasGroup.interactable = true;
                empty._CanvasGroup.blocksRaycasts = true;
                float time = 0f;
                while (time < _gen.fadeInDuration)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / _gen.fadeInDuration);
                    empty._CanvasGroup.alpha = t;
                    yield return null;
                }
                empty._CanvasGroup.alpha = 1f;
            }

            OnComplete?.Invoke();
        }

        void SetAllVisible(bool on)
        {
            float alpha = on ? 1f : 0f;
            foreach (var t in _grid)
            {
                if (t._CanvasGroup != null)
                {
                    t._CanvasGroup.alpha = alpha;
                    t._CanvasGroup.interactable = on;
                    t._CanvasGroup.blocksRaycasts = on;
                }
            }
        }
    }
}
