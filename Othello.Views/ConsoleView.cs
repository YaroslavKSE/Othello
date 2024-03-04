using Othello.Models;
using Othello.Models.Interfaces;

namespace Othello.Views;

public class ConsoleView : IGameViewUpdater
{
    public void Update(string message)
    {
        Console.WriteLine(message);
    }

    public void DisplayBoard(CellState[,] board)
    {
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                var cell = board[row, col];
                char symbol = cell switch
                {
                    CellState.Black => 'B',
                    CellState.White => 'W',
                    _ => '-'
                };
                Console.Write($"{symbol} ");
            }
            Console.WriteLine();
        }
    }

    // Additional methods to interact with the user can be added here
    // For example, displaying the board, showing error messages, etc.
}