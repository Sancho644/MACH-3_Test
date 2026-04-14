using Core.Match3.Hint;

namespace Core.Match3.Board
{
    public interface IBoardState
    {
        public bool IsBusy { get; }
        public bool HasMoves { get; }
        public bool TryFindBestMatchMove(out MoveHint hint);
        public bool IsInside(int x, int y);
    }
}