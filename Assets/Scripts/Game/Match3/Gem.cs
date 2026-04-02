namespace Game.Match3
{
    public class Gem
    {
        public GemType Type { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Gem(GemType type, int x, int y)
        {
            Type = type;
            X = x;
            Y = y;
        }

        public void SetType(GemType type)
        {
            Type = type;
        }
    }
}
