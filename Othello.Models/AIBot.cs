namespace Othello.Models;

public class AIBot : Player
{
    public AIBot(CellState color) : base(color)
    {
    }

    // The AIBot autonomously chooses a valid move.
    public override (int, int) MakeMove(Board board)
    {
        for (var row = 0; row < board.Size; row++)
        for (var col = 0; col < board.Size; col++)
            if (board.IsValidMove(row, col, Color))
                return (row, col); // Returns the first valid move found

        // Fallback if no valid move found, should not happen in a correctly implemented Othello game
        throw new InvalidOperationException("AI Bot could not find a valid move.");
    }
}