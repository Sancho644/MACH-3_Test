using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaticData;
using UnityEngine;

namespace Game.Match3
{
    [RequireComponent(typeof(BoardView))]
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private int spawnYOffset = 2;

        private IStaticDataService _staticDataService;
        private BoardStaticData _staticData;

        private readonly MoveValidator _moveValidator = new();
        private readonly BoardResolver _resolver = new();
        
        private BoardModel _model;
        private Vector2Int? _selectedCell;
        private bool _isBusy;
        private int _movesRemaining;
        private int _score;

        public bool IsBusy => _isBusy;
        public bool HasMoves => _movesRemaining > 0;
        public int MovesRemaining => _movesRemaining;
        public int Score => _score;

        public void Initialize(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _staticData = _staticDataService.GetBoardConfig();

            if (_staticData == null || boardView == null)
                return;

            _model = new BoardModel(_staticData.Width, _staticData.Height, _staticData.GemTypesCount);
            _model.InitializeNoMatches();
            _movesRemaining = _staticData.InitialMoves;
            _score = 0;

            InitializeBoardView();
        }

        public bool TrySwapAnimated(Vector2Int first, Vector2Int second, float duration, Ease ease)
        {
            if (_model == null || boardView == null || _isBusy || !HasMoves)
                return false;

            if (!_model.IsInside(first.x, first.y) || !_model.IsInside(second.x, second.y))
                return false;

            if (!_moveValidator.IsAdjacent(first, second))
                return false;

            if (!_moveValidator.HasMatchAfterSwap(_model, first, second))
                return false;

            SpendMove(1);
            _model.Swap(first.x, first.y, second.x, second.y);
            boardView.SwapViews(first, second);
            _isBusy = true;

            boardView.AnimateSwap(first, second, duration, ease, () => { StartCoroutine(ResolveCascadesAnimated()); });

            return true;
        }

        public bool TryExplodeCell(Vector2Int cell)
        {
            if (_model == null || boardView == null || _isBusy || !HasMoves)
                return false;
            if (!_model.IsInside(cell.x, cell.y))
                return false;
            if (_model.Gems[cell.x, cell.y] == null)
                return false;

            SpendMove(1);
            _isBusy = true;
            StartCoroutine(ExplodeCellRoutine(cell));
            return true;
        }

        private IEnumerator ResolveCascadesAnimated()
        {
            while (true)
            {
                List<HashSet<Vector2Int>> groups = _resolver.FindMatchGroups(_model);
                if (groups.Count == 0)
                    break;

                HashSet<Vector2Int> allMatches = new HashSet<Vector2Int>();
                int rewardMoves = 0;
                for (int i = 0; i < groups.Count; i++)
                {
                    HashSet<Vector2Int> group = groups[i];
                    if (group.Count >= 3)
                    {
                        int movesAdded = GetMovesForMatch(group.Count);
                        rewardMoves += movesAdded;
                        Debug.Log($"Match count: {group.Count}, moves added: {movesAdded}");
                        AddScore(group.Count);
                    }

                    allMatches.UnionWith(group);
                }

                AddMoves(rewardMoves);

                var explosion = boardView.PlayMatchExplosion(allMatches);
                if (explosion != null)
                    yield return explosion.WaitForCompletion();

                _resolver.RemoveMatches(_model, allMatches);
                _resolver.Collapse(_model);
                _resolver.Refill(_model);

                var fall = boardView.SyncToModelAnimated(spawnYOffset);
                if (fall != null)
                    yield return fall.WaitForCompletion();
                else
                    yield return null;
            }

            _isBusy = false;
        }

        private void InitializeBoardView()
        {
            boardView.Init(_model, _staticData);
            boardView.BuildCells();
            boardView.SyncToModel();
        }

        private IEnumerator ExplodeCellRoutine(Vector2Int cell)
        {
            var single = new HashSet<Vector2Int> { cell };
            var explosion = boardView.PlayMatchExplosion(single);
            if (explosion != null)
                yield return explosion.WaitForCompletion();

            _resolver.RemoveMatches(_model, single);
            _resolver.Collapse(_model);
            _resolver.Refill(_model);

            var fall = boardView.SyncToModelAnimated(spawnYOffset);
            if (fall != null)
                yield return fall.WaitForCompletion();
            else
                yield return null;

            yield return ResolveCascadesAnimated();
        }

        private void SpendMove(int amount)
        {
            _movesRemaining = Mathf.Max(0, _movesRemaining - amount);
            Debug.Log($"Moves remaining: {_movesRemaining}");
        }

        private void AddMoves(int amount)
        {
            if (amount <= 0)
                return;
            _movesRemaining += amount;
            Debug.Log($"Moves remaining: {_movesRemaining}");
        }

        private void AddScore(int matchCount)
        {
            if (matchCount < 3 || _staticData == null)
                return;

            var added = _staticData.Match3Score + (matchCount - 3) * _staticData.ScoreStep;
            _score += added;
            Debug.Log($"Match count: {matchCount}, score added: {added}");
            Debug.Log($"Score: {_score}");
        }

        private int GetMovesForMatch(int matchCount)
        {
            if (_staticData == null || matchCount < 3)
                return 0;

            return _staticData.Match3Moves + (matchCount - 3) * _staticData.MoveStep;
        }
    }
}