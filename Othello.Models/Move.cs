namespace Othello.Models
{
    public class Move
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public CellState PlayerColor { get; private set; }
        public List<(int Row, int Col)> FlippedPieces { get; private set; }

        public Move(int row, int col, CellState playerColor, List<(int Row, int Col)>? flippedPieces)
        {
            Row = row;
            Col = col;
            PlayerColor = playerColor;
            FlippedPieces = flippedPieces ?? new List<(int, int)>();
        }
    }
}