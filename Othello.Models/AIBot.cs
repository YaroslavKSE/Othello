using Othello.Models.Interfaces;

namespace Othello.Models;

public class AIBot : Player
{
    public AIBot(CellState color, IUndoRequestListener listener) : base(color)
    {
        _listener = listener;
    }

    private readonly Random _random = new();
    private readonly IUndoRequestListener _listener;

    // MakeMove is now an async method and returns a Task
    public override async Task MakeMoveAsync(Game gameBoard)
    {
        var cts = new CancellationTokenSource();
        var undoRequestedTask = _listener.UndoKeyPressedAsync(cts.Token);
        var simulateDelayTask = SimulateAiDelayAsync();

        var completedTask = await Task.WhenAny(undoRequestedTask, simulateDelayTask);

        if (completedTask == simulateDelayTask)
            // If simulateDelayTask completes first, cancel the undo request
            await cts.CancelAsync();

        if (completedTask == undoRequestedTask)
        {
            gameBoard.UndoMove();
            return;
        }

        var availableMoves = gameBoard.Board.GetAvailableMoves(Color);
        if (availableMoves != null && availableMoves.Count > 0)
        {
            var randomInteger = _random.Next(0, availableMoves.Count);
            var randomMove = availableMoves[randomInteger];
            gameBoard.MakeMove(randomMove.Item1, randomMove.Item2);
        }
        else
        {
            // Fallback if no valid move found
            throw new InvalidOperationException("AI Bot could not find a valid move.");
        }
    }

    public override string GetTurnMessageNotification()
    {
        return $"AI Bot {Color} is thinking...";
    }

    private async Task SimulateAiDelayAsync()
    {
        // Introduce a random delay between 1 and 3 seconds
        var delay = _random.Next(1000, 3001); // Milliseconds
        await Task.Delay(delay);
    }
}