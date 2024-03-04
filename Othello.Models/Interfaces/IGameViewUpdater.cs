namespace Othello.Models.Interfaces;

public interface IGameViewUpdater
{
    void Update(string message);
    void DisplayBoard(CellState[,] board);
}