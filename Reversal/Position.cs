namespace Reversal
{
    public struct Position
    {
        public Position(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}