using System.Collections.Generic;
using Core.Match3.Board;
using Core.Match3.Gem;
using UnityEngine;

namespace Core.Match3
{
    public class MatchFinder
    {
        public HashSet<Vector2Int> FindMatches(BoardModel model)
        {
            var matches = new HashSet<Vector2Int>();

            ScanDirection(model, 1, 0, matches);
            ScanDirection(model, 0, 1, matches);
            ScanDirection(model, 1, 1, matches);
            ScanDirection(model, 1, -1, matches);

            return matches;
        }

        private void ScanDirection(BoardModel model, int dx, int dy, HashSet<Vector2Int> matches)
        {
            for (var y = 0; y < model.Height; y++)
            {
                for (var x = 0; x < model.Width; x++)
                {
                    if (!IsStartOfLine(model, x, y, dx, dy))
                        continue;

                    var runLength = 0;
                    var cx = x;
                    var cy = y;
                    GemType? runType = null;

                    while (model.IsInside(cx, cy))
                    {
                        var gem = model.Gems[cx, cy];
                        if (gem == null)
                            break;

                        if (runType == null)
                            runType = gem.Type;
                        else if (gem.Type != runType.Value)
                            break;

                        runLength++;
                        cx += dx;
                        cy += dy;
                    }

                    if (runLength >= 3)
                    {
                        var rx = x;
                        var ry = y;
                        for (var i = 0; i < runLength; i++)
                        {
                            matches.Add(new Vector2Int(rx, ry));
                            rx += dx;
                            ry += dy;
                        }
                    }
                }
            }
        }

        private bool IsStartOfLine(BoardModel model, int x, int y, int dx, int dy)
        {
            var px = x - dx;
            var py = y - dy;
            if (!model.IsInside(x, y))
                return false;
            if (!model.IsInside(px, py))
                return true;

            var current = model.Gems[x, y];
            var prev = model.Gems[px, py];
            if (current == null || prev == null)
                return true;

            return current.Type != prev.Type;
        }
    }
}
