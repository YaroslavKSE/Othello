using Othello.Models;

namespace Othello.Controllers.Interfaces;

public interface IConsoleInputController
{
    Task<(int, int)> GetMoveInputAsync();
    string GetGameModeInput();
    bool AskPlayAgain();
}