using System.Collections.Generic;
using Core.Match3.Gem;
using UnityEngine;

namespace Core.Match3.Board
{
    public class BoardModel
    {
        public int Width { get; }
        public int Height { get; }
        public int TypesCount { get; }

        public GemData[,] Gems { get; }

        public BoardModel(int width, int height, int typesCount)
        {
            Width = width;
            Height = height;
            TypesCount = typesCount;
            Gems = new GemData[width, height];
        }

        public void InitializeNoMatches()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var type = GetRandomTypeAvoidingInitialMatch(x, y);
                    Gems[x, y] = new GemData(type, x, y);
                }
            }
        }

        public bool IsInside(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public void Swap(int ax, int ay, int bx, int by)
        {
            var a = Gems[ax, ay];
            var b = Gems[bx, by];
            Gems[ax, ay] = b;
            Gems[bx, by] = a;

            if (a != null)
            {
                a.X = bx;
                a.Y = by;
            }

            if (b != null)
            {
                b.X = ax;
                b.Y = ay;
            }
        }

        private GemType GetRandomTypeAvoidingInitialMatch(int x, int y)
        {
            var candidates = new List<GemType>(TypesCount);
            for (var i = 0; i < TypesCount; i++)
                candidates.Add((GemType)i);

            if (x >= 2)
            {
                var left1 = Gems[x - 1, y];
                var left2 = Gems[x - 2, y];
                if (left1 != null && left2 != null && left1.Type == left2.Type)
                    candidates.Remove(left1.Type);
            }

            if (y >= 2)
            {
                var down1 = Gems[x, y - 1];
                var down2 = Gems[x, y - 2];
                if (down1 != null && down2 != null && down1.Type == down2.Type)
                    candidates.Remove(down1.Type);
            }

            if (x >= 2 && y >= 2)
            {
                var downLeft1 = Gems[x - 1, y - 1];
                var downLeft2 = Gems[x - 2, y - 2];
                if (downLeft1 != null && downLeft2 != null && downLeft1.Type == downLeft2.Type)
                    candidates.Remove(downLeft1.Type);
            }

            if (x >= 2 && y + 2 < Height)
            {
                var upLeft1 = Gems[x - 1, y + 1];
                var upLeft2 = Gems[x - 2, y + 2];
                if (upLeft1 != null && upLeft2 != null && upLeft1.Type == upLeft2.Type)
                    candidates.Remove(upLeft1.Type);
            }

            if (candidates.Count == 0)
                return (GemType)Random.Range(0, TypesCount);

            var index = Random.Range(0, candidates.Count);
            return candidates[index];
        }
    }
}