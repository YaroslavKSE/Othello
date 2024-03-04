namespace Othello.Models
{
    public class Move
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public CellState PlayerColor { get; private set; }

        public Move(int row, int col, CellState playerColor)
        {
            Row = row;
            Col = col;
            PlayerColor = playerColor;
        }

        // Additional properties or methods for handling moves can be added here,
        // such as a method to execute the move on a board.
    }
}