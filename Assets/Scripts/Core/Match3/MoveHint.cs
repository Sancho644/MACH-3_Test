using UnityEngine;

namespace Core.Match3
{
    public readonly struct MoveHint
    {
        public Vector2Int From { get; }
        public Vector2Int To { get; }
        public int MatchCount { get; }

        public MoveHint(Vector2Int from, Vector2Int to, int matchCount)
        {
            From = from;
            To = to;
            MatchCount = matchCount;
        }
    }
}