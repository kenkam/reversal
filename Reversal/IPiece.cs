namespace Reversal
{
    public interface IPiece
    {
        Position Position { get; }
        Side Side { get; }
        void Flip();
    }
}