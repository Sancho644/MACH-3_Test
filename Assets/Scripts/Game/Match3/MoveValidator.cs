using UnityEngine;

namespace Game.Match3
{
    public class MoveValidator
    {
        private readonly MatchFinder _matchFinder = new();

        public bool IsAdjacent(Vector2Int a, Vector2Int b)
        {
            var dx = Mathf.Abs(a.x - b.x);
            var dy = Mathf.Abs(a.y - b.y);
            
            return (dx + dy) == 1;
        }

        public bool HasMatchAfterSwap(BoardModel model, Vector2Int a, Vector2Int b)
        {
            model.Swap(a.x, a.y, b.x, b.y);
            var hasMatch = _matchFinder.FindMatches(model).Count > 0;
            model.Swap(a.x, a.y, b.x, b.y);
            
            return hasMatch;
        }
    }
}