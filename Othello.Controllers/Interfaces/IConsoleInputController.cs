namespace Othello.Controllers.Interfaces;

public interface IConsoleInputController
{
    (int, int) GetMoveInput();
    string GetGameModeInput();
    bool AskPlayAgain();
}