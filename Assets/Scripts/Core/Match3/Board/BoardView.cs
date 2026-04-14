using System.Collections.Generic;
using Core.Match3.Gem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Match3.Board
{
    [RequireComponent(typeof(GemsPool))]
    public class BoardView : MonoBehaviour, IBoardView
    {
        [SerializeField] private GemsPool pool;
        [SerializeField] private RectTransform boardRoot;
        [SerializeField] private RectTransform cellsRoot;

        private GemView[,] _views;
        private BoardModel _model;
        private Dictionary<GemData, GemView> _gemToView;
        private Vector2 _boardOriginLocal;
        private BoardStaticData _staticData;

        public float GemStepX => _staticData != null ? _staticData.CellSize + _staticData.GemSpacing.x : 1f;
        public float GemStepY => _staticData != null ? _staticData.CellSize + _staticData.GemSpacing.y : 1f;
        public Image[,] CellViews { get; private set; }

        public void Init(BoardModel model, BoardStaticData staticData)
        {
            _model = model;
            _staticData = staticData;
            _views = new GemView[_model.Width, _model.Height];
            _gemToView = new Dictionary<GemData, GemView>();

            if (_staticData != null)
            {
                SetBoardOrigin();
            }

            pool.Init(_model, _staticData);
        }

        public void SyncToModel()
        {
            SyncToModelInternal(false, 0);
        }

        public Sequence SyncToModelAnimated(int spawnYOffset)
        {
            return SyncToModelInternal(true, spawnYOffset);
        }

        private Sequence SyncToModelInternal(bool animate, int spawnYOffset)
        {
            if (_model == null || _staticData == null)
                return null;

            if (_gemToView == null)
                _gemToView = new Dictionary<GemData, GemView>();

            UpdateAliveGems();

            var newViews = new GemView[_model.Width, _model.Height];
            var sequence = DOTween.Sequence();
            var hasTween = false;

            for (var x = 0; x < _model.Width; x++)
            {
                for (var y = 0; y < _model.Height; y++)
                {
                    var gem = _model.Gems[x, y];
                    if (gem == null)
                        continue;

                    if (!_gemToView.TryGetValue(gem, out GemView view) || view == null)
                    {
                        view = pool.GetView();
                        _gemToView[gem] = view;
                        SetupGem(view, gem);

                        if (animate && spawnYOffset > 0)
                        {
                            var spawnPos = CellToWorld(x, y + spawnYOffset);
                            spawnPos.z = 0f;
                            view.transform.position = spawnPos;
                        }
                        else
                        {
                            view.transform.position = CellToWorld(x, y);
                        }
                    }
                    else
                    {
                        SetupGem(view, gem);
                    }

                    if (view.TryGetComponent(out RectTransform rect))
                        rect.sizeDelta = new Vector2(_staticData.CellSize, _staticData.CellSize);

                    if (!view.gameObject.activeSelf)
                        view.gameObject.SetActive(true);

                    newViews[x, y] = view;

                    var target = CellToWorld(x, y);
                    view.transform.DOKill();
                    if (animate)
                    {
                        sequence.Join(view.GetMoveTween(target));
                        hasTween = true;
                    }
                    else
                    {
                        view.transform.position = target;
                    }
                }
            }

            _views = newViews;

            if (!hasTween)
            {
                sequence.Kill();
                return null;
            }

            return sequence;
        }

        public bool TryGetView(Vector2Int cell, out GemView view)
        {
            view = null;
            if (_model == null || _views == null || !_model.IsInside(cell.x, cell.y))
                return false;

            view = _views[cell.x, cell.y];
            return view != null;
        }

        public bool GetCellView(Vector2Int cell, out Image view)
        {
            view = null;
            if (CellViews == null)
            {
                return false;
            }

            view = CellViews[cell.x, cell.y];
            
            return view != null;
        }

        public void SwapViews(Vector2Int first, Vector2Int second)
        {
            if (_views == null)
                return;

            (_views[first.x, first.y], _views[second.x, second.y]) =
                (_views[second.x, second.y], _views[first.x, first.y]);
        }

        public Tween AnimateViewToCell(GemView view, Vector2Int cell, float duration, Ease ease)
        {
            if (view == null)
                return null;

            view.transform.DOKill();

            return view.GetMoveTween(CellToWorld(cell.x, cell.y), duration, ease);
        }

        public Sequence AnimateSwap(Vector2Int first, Vector2Int second, float duration, Ease ease,
            System.Action onComplete)
        {
            if (_views == null)
                return null;

            var firstView = _views[first.x, first.y];
            var secondView = _views[second.x, second.y];
            if (firstView == null || secondView == null)
                return null;

            firstView.transform.DOKill();
            secondView.transform.DOKill();

            var sequence = DOTween.Sequence();
            var firstTween = firstView.GetMoveTween(CellToWorld(first.x, first.y), duration, ease);
            var secondTween = secondView.GetMoveTween(CellToWorld(second.x, second.y), duration, ease);
            sequence.Join(firstTween);
            sequence.Join(secondTween);
            if (onComplete != null)
                sequence.OnComplete(() => onComplete());

            return sequence;
        }

        public Sequence PlayMatchExplosion(HashSet<Vector2Int> matches)
        {
            if (_views == null || matches == null || matches.Count == 0)
                return null;

            var sequence = DOTween.Sequence();
            var hasTween = false;

            foreach (var pos in matches)
            {
                if (!_model.IsInside(pos.x, pos.y))
                    continue;

                var view = _views[pos.x, pos.y];
                if (view == null)
                    continue;

                var tween = view.GetExplosionTween();
                if (tween != null)
                {
                    sequence.Join(tween);
                    hasTween = true;
                }
            }

            if (!hasTween)
            {
                sequence.Kill();
                return null;
            }

            sequence.OnComplete(() =>
            {
                foreach (var pos in matches)
                {
                    if (!_model.IsInside(pos.x, pos.y))
                        continue;

                    var view = _views[pos.x, pos.y];
                    if (view == null)
                        continue;

                    view.gameObject.SetActive(false);
                }
            });

            return sequence;
        }

        public Vector2Int WorldToCell(Vector2 worldPos)
        {
            var local = (Vector2)boardRoot.transform.InverseTransformPoint(worldPos) - _boardOriginLocal;
            var stepX = _staticData.CellSize + _staticData.GemSpacing.x;
            var stepY = _staticData.CellSize + _staticData.GemSpacing.y;
            var x = Mathf.FloorToInt(local.x / stepX);
            var y = Mathf.FloorToInt(local.y / stepY);

            return new Vector2Int(x, y);
        }

        public bool IsInside(Vector2Int cell)
        {
            if (_model == null)
                return false;

            return _model.IsInside(cell.x, cell.y);
        }

        public void BuildCells()
        {
            if (_model == null || _staticData == null || _staticData.CellPrefab == null || CellViews != null)
                return;

            CellViews = new Image[_model.Width, _model.Height];
            if (cellsRoot == null)
            {
                var root = new GameObject("CellsRoot", typeof(RectTransform));
                root.transform.SetParent(boardRoot, false);
                cellsRoot = root.GetComponent<RectTransform>();
                cellsRoot.SetAsFirstSibling();
            }

            for (var x = 0; x < _model.Width; x++)
            {
                for (var y = 0; y < _model.Height; y++)
                {
                    var cell = Instantiate(_staticData.CellPrefab, cellsRoot);
                    if (_staticData.CellSprite != null)
                        cell.sprite = _staticData.CellSprite;

                    var rect = cell.rectTransform;
                    rect.sizeDelta = new Vector2(_staticData.CellVisualSize, _staticData.CellVisualSize);
                    rect.position = CellToCellWorld(x, y);
                    CellViews[x, y] = cell;
                }
            }
        }

        private Vector3 CellToWorld(int x, int y)
        {
            var stepX = _staticData.CellSize + _staticData.GemSpacing.x;
            var stepY = _staticData.CellSize + _staticData.GemSpacing.y;
            var local = new Vector3(
                _boardOriginLocal.x + (x + 0.5f) * stepX,
                _boardOriginLocal.y + (y + 0.5f) * stepY,
                0f);
            return boardRoot.transform.TransformPoint(local);
        }

        private void SetBoardOrigin()
        {
            var center = (Vector2)boardRoot.TransformPoint(_staticData.BoardOffset);
            var gemStep = new Vector2(_staticData.CellSize + _staticData.GemSpacing.x,
                _staticData.CellSize + _staticData.GemSpacing.y);
            var size = new Vector2(_staticData.Width * gemStep.x, _staticData.Height * gemStep.y);
            var origin = center - size * 0.5f;

            _boardOriginLocal = boardRoot.transform.InverseTransformPoint(origin);
        }

        private void UpdateAliveGems()
        {
            var alive = new HashSet<GemData>();
            for (var x = 0; x < _model.Width; x++)
            {
                for (var y = 0; y < _model.Height; y++)
                {
                    var gem = _model.Gems[x, y];
                    if (gem != null)
                        alive.Add(gem);
                }
            }

            if (_gemToView.Count > 0)
            {
                var toRemove = new List<GemData>();
                foreach (var pair in _gemToView)
                {
                    if (alive.Contains(pair.Key))
                        continue;

                    if (pair.Value != null)
                    {
                        pool.ReleaseView(pair.Value);
                    }

                    toRemove.Add(pair.Key);
                }

                for (var i = 0; i < toRemove.Count; i++)
                {
                    _gemToView.Remove(toRemove[i]);
                }
            }
        }

        private void SetupGem(GemView view, GemData gemData)
        {
            view.ResetVisuals();
            view.SetGem(gemData);
            var spriteIndex = Mathf.Clamp((int)gemData.Type, 0, _staticData.GemSprites.Length - 1);
            view.SetSprite(_staticData.GemSprites[spriteIndex]);
        }

        private Vector3 CellToCellWorld(int x, int y)
        {
            var stepX = _staticData.CellVisualSize + _staticData.CellSpacing.x;
            var stepY = _staticData.CellVisualSize + _staticData.CellSpacing.y;
            var local = new Vector3(
                _boardOriginLocal.x + (x + 0.5f) * stepX,
                _boardOriginLocal.y + (y + 0.5f) * stepY,
                0f);

            return boardRoot.transform.TransformPoint(local);
        }
    }
}