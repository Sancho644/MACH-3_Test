using Core.Match3.Board;
using Core.Match3.Board.GameEvents;
using Core.Match3.GameEvents;
using GameEvents;
using UnityEngine;

namespace Core.Match3.Hint
{
    public class HintController : MonoBehaviour
    {
        private IGameEventsDispatcher _gameEventsDispatcher;
        private IBoardState _boardState;
        private IBoardView _boardView;
        private HintView _hintView;

        public void Initialize(IBoardState boardState, IGameEventsDispatcher gameEventsDispatcher, IBoardView boardView)
        {
            _boardState = boardState;
            _boardView = boardView;
            _gameEventsDispatcher = gameEventsDispatcher;

            _gameEventsDispatcher.AddListener<StartInputEvent>(OnStartInput);
            _gameEventsDispatcher.AddListener<WindowInitializationCompleteEvent>(OnBoardReady);

            _hintView = new HintView(_boardState, _boardView);
        }

        private void OnDestroy()
        {
            _gameEventsDispatcher.RemoveListener<StartInputEvent>(OnStartInput);
            _gameEventsDispatcher.RemoveListener<WindowInitializationCompleteEvent>(OnBoardReady);
        }

        private bool ShowHint()
        {
            if (_boardView == null)
                return false;

            if (!TryGetHint(out var hint))
                return false;

            return _hintView.ShowHint(hint.From, hint.To);
        }

        private void ClearHint()
        {
            if (_boardView == null)
                return;

            _hintView.ClearHint();
        }

        private bool TryGetHint(out MoveHint hint)
        {
            hint = default;

            if (_boardState.IsBusy || !_boardState.HasMoves)
                return false;

            return _boardState.TryFindBestMatchMove(out hint);
        }

        private void OnBoardReady(WindowInitializationCompleteEvent @event)
        {
            ShowHint();
        }

        private void OnStartInput(StartInputEvent @event)
        {
            ClearHint();
        }
    }
}