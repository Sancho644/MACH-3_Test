using UnityEngine;

namespace Game.Match3
{
    public class MoveValidator
    {
        private readonly MatchFinder _matchFinder = new();

        private readonly Vector2Int[] _directions =
        {
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.down
        };

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

        public bool TryFindBestMatchMove(BoardModel model, out MoveHint hint)
        {
            hint = default;

            if (model == null)
                return false;

            var bestMatchCount = 0;
            var found = false;

            for (var x = 0; x < model.Width; x++)
            {
                for (var y = 0; y < model.Height; y++)
                {
                    if (model.Gems[x, y] == null)
                        continue;

                    var from = new Vector2Int(x, y);

                    for (var i = 0; i < _directions.Length; i++)
                    {
                        var to = from + _directions[i];
                        if (!model.IsInside(to.x, to.y) || model.Gems[to.x, to.y] == null)
                            continue;

                        var matchCount = GetMatchCountAfterSwap(model, from, to);
                        if (matchCount <= 0 || matchCount <= bestMatchCount)
                            continue;

                        bestMatchCount = matchCount;
                        hint = new MoveHint(from, to, matchCount);
                        found = true;
                    }
                }
            }

            return found;
        }

        private int GetMatchCountAfterSwap(BoardModel model, Vector2Int a, Vector2Int b)
        {
            model.Swap(a.x, a.y, b.x, b.y);
            var matchCount = _matchFinder.FindMatches(model).Count;
            model.Swap(a.x, a.y, b.x, b.y);

            return matchCount;
        }
    }
}
