namespace Othello.Controllers.Interfaces;

public interface IGameController
{
    Task StartGame();
    void SimulateAiDelay();
    Task<bool> WaitForUndoRequest(int timeout);
}