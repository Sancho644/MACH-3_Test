using UnityEngine;
using UnityEngine.UI;

namespace Core.Match3
{
    [CreateAssetMenu(fileName = "BoardStaticData", menuName = "StaticData/BoardStaticData")]
    public class BoardStaticData : ScriptableObject
    {
        [Tooltip("Количество гемов во горизонтали")]
        [Min(3)] public int Width = 8;
        [Tooltip("Количество гемов во вертикали")]
        [Min(3)] public int Height = 8;
        [Tooltip("Количество типов гемов")]
        [Min(3)] public int GemTypesCount = 5;
        [Tooltip("Начальное количество ходов")]
        [Min(0)] public int InitialMoves = 20;
        [Tooltip("Количество очков за совпадение 3х гемов")]
        [Min(0)] public int Match3Score = 100;
        [Tooltip("Количество очков, которые выдаются за каждое последующее совпадение гемов больше 3х")]
        [Min(0)] public int ScoreStep = 100;
        [Tooltip("Количество ходов которые даются за совподение 3х гемов")]
        [Min(0)] public int Match3Moves = 2;
        [Tooltip("Количество ходов которые выдаются за каждое последеющее совподение гемов больше 3х")]
        [Min(0)] public int MoveStep = 1;
        [Tooltip("Максимальное количество строчек в окне рекордов")]
        [Min(0)] public int MaxRecords = 10;

        [Min(0.1f)] public float CellSize = 1f;
        [Min(0.1f)] public float CellVisualSize = 1f;
        public Vector2 GemSpacing = Vector2.zero;
        public Vector2 CellSpacing = Vector2.zero;
        public Vector2 BoardOffset = Vector2.zero;

        public Image CellPrefab;
        public Sprite CellSprite;

        public GemView GemPrefab;
        public Sprite[] GemSprites;
    }
}
