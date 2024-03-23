using Othello.Models.Interfaces;

namespace Othello.Models;

public class HumanPlayer : Player
{
    public HumanPlayer(CellState color, IPlayerInputGetter inputGetter) : base(color)
    {
        _inputGetter = inputGetter;
    }

    private readonly IPlayerInputGetter _inputGetter;

    public override Task MakeMoveAsync(Game gameBoard)
    {
        var (row, col) = _inputGetter.GetMoveInput();
        gameBoard.MakeMove(row - 1, col - 1);
        return Task.CompletedTask;
    }

    public override string GetTurnMessageNotification()
    {
        return $"Player {Color}'s turn. Please enter your move (row col):";
    }
}