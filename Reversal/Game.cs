namespace Reversal
{
    public sealed class Game
    {
        internal static Side startingSide = Side.Black;
        private readonly IBoard board;

        public Game(IBoard board)
        {
            this.board = board;
            Turn = startingSide;
        }

        public Side Turn { get; private set; }

        public void PlayPieceAt(Position position)
        {
            var piece = new Piece(position, Turn);
            if (!board.CanPlay(piece))
            {
                return;
            }

            board.Play(piece);
            ChangeTurn();
        }

        private void ChangeTurn()
        {
            Turn = Turn == Side.Black ? Side.White : Side.Black;
        }
    }
}