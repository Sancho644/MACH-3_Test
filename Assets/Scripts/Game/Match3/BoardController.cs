using System.Collections;
using System.Collections.Generic;
using Data.Services;
using DG.Tweening;
using Game.Match3.GameEvents;
using GameEvents;
using StaticData;
using UnityEngine;

namespace Game.Match3
{
    [RequireComponent(typeof(BoardView))]
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private int spawnYOffset = 2;

        private IGameEventsDispatcher _gameEventsDispatcher;
        private IPlayerStatsService _playerStatsService;
        private IStaticDataService _staticDataService;
        private BoardStaticData _staticData;

        private readonly MoveValidator _moveValidator = new();
        private readonly BoardResolver _resolver = new();

        private BoardModel _model;
        private Vector2Int? _selectedCell;
        private bool _isBusy;
        private int _movesRemaining;

        public bool IsBusy => _isBusy;
        public bool HasMoves => _movesRemaining > 0;

        public void Initialize(IStaticDataService staticDataService, IPlayerStatsService playerStatsService,
            IGameEventsDispatcher gameEventsDispatcher)
        {
            _staticDataService = staticDataService;
            _playerStatsService = playerStatsService;
            _gameEventsDispatcher = gameEventsDispatcher;
            _staticData = _staticDataService.GetBoardConfig();

            if (_staticData == null || boardView == null || _playerStatsService == null)
                return;

            _model = new BoardModel(_staticData.Width, _staticData.Height, _staticData.GemTypesCount);
            _model.InitializeNoMatches();
            _movesRemaining = _playerStatsService.Moves;

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
            if (_movesRemaining <= 0)
            {
                _gameEventsDispatcher.Dispatch(new OutOfMovesEvent());
            }
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
            _playerStatsService.SpendMoves(amount);
            _movesRemaining = _playerStatsService.Moves;
            _gameEventsDispatcher.Dispatch(new PlayerStatsChangedEvent());
            
            Debug.Log($"Moves remaining: {_movesRemaining}");
        }

        private void AddMoves(int amount)
        {
            _playerStatsService.AddMoves(amount);
            _movesRemaining = _playerStatsService.Moves;
            _gameEventsDispatcher.Dispatch(new PlayerStatsChangedEvent());
            
            Debug.Log($"Moves remaining: {_movesRemaining}");
        }

        private void AddScore(int matchCount)
        {
            if (matchCount < 3 || _staticData == null)
                return;

            var added = _staticData.Match3Score + (matchCount - 3) * _playerStatsService.Score;

            _playerStatsService.AddScore(added);
            _gameEventsDispatcher.Dispatch(new PlayerStatsChangedEvent());
            
            Debug.Log($"Match count: {matchCount}, score added: {added}");
        }

        private int GetMovesForMatch(int matchCount)
        {
            if (_staticData == null || matchCount < 3)
                return 0;

            return _staticData.Match3Moves + (matchCount - 3) * _staticData.MoveStep;
        }
    }
}