using Core.Match3.Gem;
using UnityEngine;

namespace Core.Match3.Board
{
    public interface IBoardView
    {
        public bool TryGetView(Vector2Int cell, out GemView view);
    }
}