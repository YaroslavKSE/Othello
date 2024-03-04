namespace Othello.Models
{
    public class AIBot : Player
    {
        public AIBot(CellState color) : base(color) { }

        public override (int, int) MakeMove(Board board)
        {
            // Simple AI logic to choose a move
            // For demonstration purposes, this might just pick the first available valid move
            // A real AI would have more complex logic to evaluate the best move

            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    if (board.IsValidMove(row, col, Color))
                    {
                        return (row, col);
                    }
                }
            }

            // Fallback if no valid move found, should not happen in a correctly implemented Othello game
            throw new InvalidOperationException("AI Bot could not find a valid move.");
        }
    }
}