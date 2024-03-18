namespace Othello.Models;

public class HumanPlayer : Player
{
    public HumanPlayer(CellState color) : base(color)
    {
    }

    public override (int, int) MakeMove(Board board)
    {
        throw new InvalidOperationException("HumanPlayer should not make moves directly.");
    }
}