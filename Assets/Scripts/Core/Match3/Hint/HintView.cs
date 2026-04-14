using Core.Match3.Board;
using Core.Match3.Gem;
using UnityEngine;

namespace Core.Match3.Hint
{
    public class HintView
    {
        private readonly IBoardState _boardState;
        private readonly IBoardView _boardView;
        
        private GemView _hintGemView;

        public HintView(IBoardState boardState, IBoardView boardView)
        {
            _boardState = boardState;
            _boardView = boardView;
        }
        
        public bool ShowHint(Vector2Int from, Vector2Int to)
        {
            ClearHint();

            if (!_boardView.TryGetView(from, out var gemView))
                return false;
            if (!_boardState.IsInside(to.x, to.y))
                return false;

            _hintGemView = gemView;

            var hintDirection = (Vector2)(to - from);

            gemView.GetHintTween(hintDirection);

            return true;
        }

        public void ClearHint()
        {
            if (_hintGemView != null)
                _hintGemView.ResetVisuals();

            _hintGemView = null;
        }
    }
}