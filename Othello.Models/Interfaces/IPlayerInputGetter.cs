namespace Othello.Models.Interfaces;

public interface IPlayerInputGetter
{
    (int, int) GetMoveInput();
}