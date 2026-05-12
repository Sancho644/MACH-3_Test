using System.Collections;
using System.Collections.Generic;
using Core.Match3.GameEvents;
using Core.Match3.Hint;
using Core.Yandex.Services;
using Data.Services;
using DG.Tweening;
using Game.Services.GameEvents;
using GameEvents;
using StaticData;
using UI.Windows.MainMenu;
using UnityEngine;

namespace Core.Match3.Board
{
    [RequireComponent(typeof(BoardView))]
    public class BoardController : MonoBehaviour, IBoardState
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private int spawnYOffset = 2;

        private IGameEventsDispatcher _gameEventsDispatcher;
        private ILeaderboardService _leaderboardService;
        private IPlayerStatsService _playerStatsService;
        private IStaticDataService _staticDataService;
        private BoardStaticData _staticData;

        private readonly MoveValidator _moveValidator = new();
        private readonly BoardResolver _resolver = new();

        private int _movesRemaining;
        private BoardModel Model { get; set; }

        public bool IsBusy { get; private set; }
        public bool HasMoves => _movesRemaining > 0;

        public void Initialize(IStaticDataService staticDataService, IPlayerStatsService playerStatsService,
            IGameEventsDispatcher gameEventsDispatcher, ILeaderboardService leaderboardService)
        {
            _staticDataService = staticDataService;
            _playerStatsService = playerStatsService;
            _gameEventsDispatcher = gameEventsDispatcher;
            _leaderboardService = leaderboardService;

            _staticData = _staticDataService.GetBoardConfig();

            if (_staticData == null || boardView == null || _playerStatsService == null)
                return;

            Model = new BoardModel(_staticData.Width, _staticData.Height, _staticData.GemTypesCount);
            Model.InitializeNoMatches();
            _movesRemaining = _playerStatsService.Moves;

            InitializeBoardView();
        }

        public bool TrySwapAnimated(Vector2Int first, Vector2Int second, float duration, Ease ease)
        {
            if (Model == null || boardView == null || IsBusy || !HasMoves)
                return false;

            if (!Model.IsInside(first.x, first.y) || !Model.IsInside(second.x, second.y))
                return false;

            if (!_moveValidator.IsAdjacent(first, second))
                return false;

            if (!_moveValidator.HasMatchAfterSwap(Model, first, second))
                return false;

            SpendMove(1);
            Model.Swap(first.x, first.y, second.x, second.y);
            boardView.SwapViews(first, second);
            IsBusy = true;

            boardView.AnimateSwap(first, second, duration, ease, () => { StartCoroutine(ResolveCascadesAnimated()); });

            return true;
        }

        public bool TryExplodeCell(Vector2Int cell)
        {
            if (Model == null || boardView == null || IsBusy || !HasMoves)
                return false;
            if (!Model.IsInside(cell.x, cell.y))
                return false;
            if (Model.Gems[cell.x, cell.y] == null)
                return false;

            SpendMove(1);
            IsBusy = true;

            StartCoroutine(ExplodeCellRoutine(cell));

            return true;
        }

        public bool TryFindBestMatchMove(out MoveHint hint)
        {
            hint = default;

            if (Model == null)
            {
                return false;
            }

            return _moveValidator.TryFindBestMatchMove(Model, out hint);
        }

        public bool IsInside(int x, int y)
        {
            return Model.IsInside(x, y);
        }

        private IEnumerator ResolveCascadesAnimated()
        {
            while (true)
            {
                List<HashSet<Vector2Int>> groups = _resolver.FindMatchGroups(Model);
                if (groups.Count == 0)
                    break;

                var allMatches = new HashSet<Vector2Int>();
                var rewardMoves = 0;
                for (var i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];
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

                _resolver.RemoveMatches(Model, allMatches);
                _resolver.Collapse(Model);
                _resolver.Refill(Model);

                var fall = boardView.SyncToModelAnimated(spawnYOffset);
                if (fall != null)
                    yield return fall.WaitForCompletion();
                else
                    yield return null;
            }

            IsBusy = false;
            CheckRecordsStatus();
        }

        private void CheckRecordsStatus()
        {
            if (_movesRemaining > 0)
            {
                return;
            }

            _leaderboardService.LoadTop(OnLoaded);
          
        }

        private void OnLoaded(List<RecordEntry> recordEntries)
        {
            var isEnoughScore = _leaderboardService.IsEnoughScore(_playerStatsService.Score, recordEntries);
            if (_movesRemaining <= 0 && isEnoughScore)
            {
                _leaderboardService.WriteScore(_playerStatsService.Score);
                _gameEventsDispatcher.Dispatch(new GameActionEvent(GameActionType.Records));
            }

            if (_movesRemaining <= 0 && !isEnoughScore)
            {
                _gameEventsDispatcher.Dispatch(new OutOfMovesEvent());
            }
        }

        private void InitializeBoardView()
        {
            boardView.Init(Model, _staticData);
            boardView.BuildCells();
            boardView.SyncToModel();
        }

        private IEnumerator ExplodeCellRoutine(Vector2Int cell)
        {
            var single = new HashSet<Vector2Int> { cell };
            var explosion = boardView.PlayMatchExplosion(single);
            if (explosion != null)
                yield return explosion.WaitForCompletion();

            _resolver.RemoveMatches(Model, single);
            _resolver.Collapse(Model);
            _resolver.Refill(Model);

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

            Debug.Log($"Moves remaining: {_movesRemaining}");
        }

        private void AddMoves(int amount)
        {
            _playerStatsService.AddMoves(amount);
            _movesRemaining = _playerStatsService.Moves;

            Debug.Log($"Moves remaining: {_movesRemaining}");
        }

        private void AddScore(int matchCount)
        {
            if (matchCount < 3 || _staticData == null)
                return;

            var added = _staticData.Match3Score + (matchCount - 3) * _playerStatsService.Score;

            _playerStatsService.AddScore(added);

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