namespace Othello.Models;

public class HumanPlayer : Player
{
    public HumanPlayer(CellState color) : base(color)
    {
    }

    public override Task<(int, int)> MakeMoveAsync(Board board)
    {
        throw new InvalidOperationException("HumanPlayer should not make moves directly.");
    }
}