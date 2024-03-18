namespace Othello.Models;

public abstract class Player
{
    public CellState Color { get; private set; }

    protected Player(CellState color)
    {
        if (color == CellState.Empty)
            throw new ArgumentException("Player color must be Black or White.", nameof(color));
        Color = color;
    }

    // Abstract method for making a move, to be implemented by subclasses
    public abstract (int, int) MakeMove(Board board);
}