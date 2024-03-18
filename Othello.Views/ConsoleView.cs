using Othello.Models;
using Othello.Models.Interfaces;

namespace Othello.Views;

public class ConsoleView : IGameViewUpdater
{
    public void Update(string message)
    {
        Console.WriteLine(message);
    }
    
    public void DisplayBoard(CellState[,] board, List<(int, int)>? hints)
    {
        int size = board.GetLength(0); // Assuming the board is square

        // Display the top border
        Console.Write("   "); // Space for row numbers
        for (int col = 0; col < size; col++)
        {
            Console.Write($"  {col + 1}"); // Display column numbers as coordinates
        }

        Console.WriteLine();

        Console.Write("    "); // Align with the board
        Console.WriteLine(new string('=', size * 3)); // Top border based on board size

        for (int row = 0; row < size; row++)
        {
            // Display row numbers as coordinates, right aligned
            Console.Write($"{row + 1}".PadLeft(2) + " |"); // PadLeft for alignment

            for (int col = 0; col < size; col++)
            {
                var cell = board[row, col];
                char symbol = hints != null && hints.Contains((row, col)) ? '*' : cell switch
                {
                    CellState.Black => 'B',
                    CellState.White => 'W',
                    _ => '-'
                };
                Console.Write($" {symbol} "); // Display the cell state with spaces for readability
            }

            Console.WriteLine("|"); // Right border
        }

        Console.Write("    "); // Align with the board
        Console.WriteLine(new string('=', size * 3)); // Bottom border based on board size
    }
}