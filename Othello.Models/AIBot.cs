using System;
using System.Threading.Tasks;

namespace Othello.Models;

public class AIBot : Player
{
    public AIBot(CellState color) : base(color)
    {
    }
    
    // MakeMove is now an async method and returns a Task
    public override async Task<(int, int)> MakeMoveAsync(Board board)
    {
        await SimulateAiDelayAsync(); // Simulate thinking delay asynchronously

        for (var row = 0; row < board.Size; row++)
        for (var col = 0; col < board.Size; col++)
            if (board.IsValidMove(row, col, Color))
                return (row, col); // Returns the first valid move found

        // Fallback if no valid move found, should not happen in a correctly implemented Othello game
        throw new InvalidOperationException("AI Bot could not find a valid move.");
    }

    private async Task SimulateAiDelayAsync()
    {
        // Introduce a random delay between 1 and 3 seconds
        var rand = new Random();
        var delay = rand.Next(1000, 3001); // Milliseconds
        await Task.Delay(delay);
    }
}