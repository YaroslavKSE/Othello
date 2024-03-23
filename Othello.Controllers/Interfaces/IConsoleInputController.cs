namespace Othello.Controllers.Interfaces;

public interface IConsoleInputController
{
    string GetGameModeInput();
    bool AskPlayAgain();
}