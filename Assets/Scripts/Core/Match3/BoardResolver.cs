using System.Collections.Generic;
using UnityEngine;

namespace Core.Match3
{
    public class BoardResolver
    {
        private readonly MatchFinder _matchFinder = new();

        public List<HashSet<Vector2Int>> FindMatchGroups(BoardModel model)
        {
            var matches = FindMatches(model);
            var groups = new List<HashSet<Vector2Int>>();
            if (matches.Count == 0)
                return groups;

            var visited = new HashSet<Vector2Int>();
            Vector2Int[] directions =
            {
                new(-1, -1), new(0, -1), new(1, -1),
                new(-1, 0), new(1, 0),
                new(-1, 1), new(0, 1), new(1, 1)
            };

            foreach (var start in matches)
            {
                if (visited.Contains(start))
                    continue;

                var startGem = model.Gems[start.x, start.y];
                if (startGem == null)
                {
                    visited.Add(start);
                    continue;
                }

                var targetType = startGem.Type;
                var group = new HashSet<Vector2Int>();
                var queue = new Queue<Vector2Int>();
                
                queue.Enqueue(start);
                visited.Add(start);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    group.Add(current);

                    for (var i = 0; i < directions.Length; i++)
                    {
                        var next = current + directions[i];
                        if (!model.IsInside(next.x, next.y))
                            continue;
                        if (visited.Contains(next))
                            continue;
                        if (!matches.Contains(next))
                            continue;

                        var gem = model.Gems[next.x, next.y];
                        if (gem == null || gem.Type != targetType)
                            continue;

                        visited.Add(next);
                        queue.Enqueue(next);
                    }
                }

                if (group.Count > 0)
                    groups.Add(group);
            }

            return groups;
        }

        public void RemoveMatches(BoardModel model, HashSet<Vector2Int> matches)
        {
            foreach (var pos in matches)
            {
                model.Gems[pos.x, pos.y] = null;
            }
        }

        public void Collapse(BoardModel model)
        {
            for (var x = 0; x < model.Width; x++)
            {
                var writeY = 0;
                for (var y = 0; y < model.Height; y++)
                {
                    var gem = model.Gems[x, y];
                    if (gem == null)
                        continue;

                    if (writeY != y)
                    {
                        model.Gems[x, writeY] = gem;
                        model.Gems[x, y] = null;
                        gem.X = x;
                        gem.Y = writeY;
                    }

                    writeY++;
                }
            }
        }

        public void Refill(BoardModel model)
        {
            for (var x = 0; x < model.Width; x++)
            {
                for (var y = 0; y < model.Height; y++)
                {
                    if (model.Gems[x, y] != null)
                        continue;

                    var type = (GemType)Random.Range(0, model.TypesCount);
                    model.Gems[x, y] = new Gem(type, x, y);
                }
            }
        }

        private HashSet<Vector2Int> FindMatches(BoardModel model)
        {
            return _matchFinder.FindMatches(model);
        }
    }
}