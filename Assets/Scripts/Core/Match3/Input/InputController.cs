using Core.Match3.Board;
using Core.Match3.GameEvents;
using Core.Match3.Gem;
using DG.Tweening;
using GameEvents;
using UnityEngine;

namespace Core.Match3
{
    [RequireComponent(typeof(BoardController), typeof(BoardView))]
    public class InputController : MonoBehaviour
    {
        private enum DragAxis
        {
            None,
            Horizontal,
            Vertical
        }

        [SerializeField] private Camera worldCamera;
        [SerializeField] private BoardView boardView;
        [SerializeField] private BoardController boardController;

        [Header("Animations")] [SerializeField]
        private float dragStartThreshold = 0.2f;

        [SerializeField] private float dragFollowSpeed = 25f;
        [SerializeField] private float swapAnimationDuration = 0.2f;
        [SerializeField] private float snapBackDuration = 0.12f;
        [SerializeField] private Ease swapAnimationEase = Ease.OutQuad;
        [SerializeField] private Ease snapBackEase = Ease.OutQuad;

        private Vector2Int? _pressedCell;
        private Vector3 _pressWorld;
        private Vector3 _dragOffset;
        private DragAxis _dragAxis = DragAxis.None;
        private GemView _dragView;
        private bool _isDragging;
        private bool _isSnapping;
        private bool _initialized;
        private bool _pause;
        private int _dragOriginalSiblingIndex = -1;
        private RectTransform _boardRect;
        private Canvas _boardCanvas;
        private Camera _uiCamera;

        private IGameEventsDispatcher _gameEventsDispatcher;

        public void Initialize(IGameEventsDispatcher gameEventsDispatcher)
        {
            _gameEventsDispatcher = gameEventsDispatcher;
            _gameEventsDispatcher.AddListener<PauseInputEvent>(OnPauseInput);

            _initialized = true;
        }

        private void Awake()
        {
            if (worldCamera == null)
            {
                worldCamera = Camera.main;
            }

            if (boardView == null)
            {
                return;
            }

            _boardRect = boardView.GetComponent<RectTransform>();
            if (_boardRect == null)
            {
                return;
            }

            _boardCanvas = boardView.GetComponentInParent<Canvas>();
            if (_boardCanvas != null && _boardCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                _uiCamera = _boardCanvas.worldCamera;
            }
        }

        private void Update()
        {
            if (!_initialized)
                return;

            if (_pause)
                return;

            if (worldCamera == null || boardView == null || boardController == null)
                return;

            if (_isSnapping || boardController.IsBusy)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                TrySetupPickGem();
            }

            if (Input.GetMouseButton(0) && _pressedCell != null && _dragView != null)
            {
                TryDragGem();
            }

            if (Input.GetMouseButtonUp(0) && _pressedCell != null)
            {
                var first = _pressedCell.Value;
                if (!TryGetPointerWorld(out Vector3 world))
                    return;

                TrySwapGems(first, world);

                if (_dragView != null && _dragOriginalSiblingIndex >= 0 && !_isSnapping)
                    _dragView.transform.SetSiblingIndex(_dragOriginalSiblingIndex);

                _pressedCell = null;
                _dragView = null;
                _isDragging = false;
                _dragAxis = DragAxis.None;
                _dragOriginalSiblingIndex = -1;
            }
        }

        private void OnDestroy()
        {
            _gameEventsDispatcher.RemoveListener<PauseInputEvent>(OnPauseInput);
        }

        private void TrySwapGems(Vector2Int first, Vector3 world)
        {
            if (!_isDragging)
            {
                if (boardController.TryExplodeCell(first))
                {
                    _gameEventsDispatcher.Dispatch(new StartInputEvent());
                }
            }
            else
            {
                var second = GetAdjacentCellFromDrag(first, world - _pressWorld);
                if (!boardView.IsInside(second) ||
                    !boardController.TrySwapAnimated(first, second, swapAnimationDuration, swapAnimationEase))
                {
                    _gameEventsDispatcher.Dispatch(new StartInputEvent());

                    if (boardView.IsInside(second) && boardView.TryGetView(second, out GemView otherView))
                    {
                        var snapView = _dragView;
                        var snapSiblingIndex = _dragOriginalSiblingIndex;
                        _isSnapping = true;
                        AnimateSwapAndRevert(
                            _dragView,
                            otherView,
                            first,
                            second,
                            swapAnimationDuration,
                            swapAnimationEase,
                            snapBackDuration,
                            snapBackEase,
                            () =>
                            {
                                _isSnapping = false;
                                if (snapView != null && snapSiblingIndex >= 0)
                                    snapView.transform.SetSiblingIndex(snapSiblingIndex);
                            });
                    }
                    else
                    {
                        var snapView = _dragView;
                        var snapSiblingIndex = _dragOriginalSiblingIndex;
                        _isSnapping = true;
                        boardView.AnimateViewToCell(_dragView, first, snapBackDuration, snapBackEase)
                            .OnComplete(() =>
                            {
                                _isSnapping = false;
                                if (snapView != null && snapSiblingIndex >= 0)
                                    snapView.transform.SetSiblingIndex(snapSiblingIndex);
                            });
                    }
                }
            }
        }

        private void TryDragGem()
        {
            if (!TryGetPointerWorld(out Vector3 world))
                return;

            var delta = world - _pressWorld;
            if (!_isDragging && delta.magnitude >= dragStartThreshold)
            {
                _isDragging = true;
                _dragAxis = Mathf.Abs(delta.x) >= Mathf.Abs(delta.y) ? DragAxis.Horizontal : DragAxis.Vertical;
            }

            if (_isDragging && _pressedCell != null)
            {
                var axisDelta = GetAxisDelta(delta, _pressedCell.Value);
                var maxDistance = _dragAxis == DragAxis.Horizontal ? boardView.GemStepX : boardView.GemStepY;
                axisDelta = Vector3.ClampMagnitude(axisDelta, maxDistance);
                var target = _pressWorld + axisDelta + _dragOffset;

                _dragView.transform.position = Vector3.Lerp(
                    _dragView.transform.position,
                    target,
                    Time.deltaTime * dragFollowSpeed);
            }
        }

        private void TrySetupPickGem()
        {
            if (!boardController.HasMoves)
                return;
            if (!TryGetPointerWorld(out Vector3 world))
                return;

            var cell = boardView.WorldToCell(world);
            if (boardView.IsInside(cell) && boardView.TryGetView(cell, out GemView view))
            {
                _pressedCell = cell;
                _pressWorld = world;
                _dragView = view;
                _isDragging = false;
                _dragAxis = DragAxis.None;
                _dragOffset = view.transform.position - world;
                _dragOriginalSiblingIndex = view.transform.GetSiblingIndex();
                view.transform.SetAsLastSibling();
                view.transform.DOKill();
            }
        }

        private void OnPauseInput(PauseInputEvent @event)
        {
            _pause = @event.Pause;
        }

        private Vector2Int GetAdjacentCellFromDrag(Vector2Int origin, Vector3 delta)
        {
            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
                return new Vector2Int(origin.x + (delta.x >= 0f ? 1 : -1), origin.y);

            return new Vector2Int(origin.x, origin.y + (delta.y >= 0f ? 1 : -1));
        }

        private Vector3 GetAxisDelta(Vector3 delta, Vector2Int origin)
        {
            if (_dragAxis == DragAxis.Horizontal)
            {
                var signed = delta.x;
                var next = new Vector2Int(origin.x + (signed >= 0f ? 1 : -1), origin.y);
                if (!boardView.IsInside(next))
                    return Vector3.zero;
                return new Vector3(signed, 0f, 0f);
            }

            if (_dragAxis == DragAxis.Vertical)
            {
                var signed = delta.y;
                var next = new Vector2Int(origin.x, origin.y + (signed >= 0f ? 1 : -1));
                if (!boardView.IsInside(next))
                    return Vector3.zero;
                return new Vector3(0f, signed, 0f);
            }

            return Vector3.zero;
        }

        private bool TryGetPointerWorld(out Vector3 world)
        {
            if (_boardRect != null)
            {
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                        _boardRect,
                        Input.mousePosition,
                        _uiCamera,
                        out world))
                {
                    world.z = 0f;
                    return true;
                }
            }

            if (worldCamera == null)
            {
                world = Vector3.zero;
                return false;
            }

            world = worldCamera.ScreenToWorldPoint(Input.mousePosition);
            world.z = 0f;
            return true;
        }

        private void AnimateSwapAndRevert(
            GemView firstView,
            GemView secondView,
            Vector2Int firstCell,
            Vector2Int secondCell,
            float swapDuration,
            Ease swapEase,
            float revertDuration,
            Ease revertEase,
            System.Action onComplete)
        {
            var sequence = DOTween.Sequence();
            sequence.Join(boardView.AnimateViewToCell(firstView, secondCell, swapDuration, swapEase));
            sequence.Join(boardView.AnimateViewToCell(secondView, firstCell, swapDuration, swapEase));
            sequence.Append(boardView.AnimateViewToCell(firstView, firstCell, revertDuration, revertEase));
            sequence.Join(boardView.AnimateViewToCell(secondView, secondCell, revertDuration, revertEase));
            if (onComplete != null)
                sequence.OnComplete(() => onComplete());
        }
    }
}