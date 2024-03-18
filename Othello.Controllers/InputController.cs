using Othello.Controllers.Interfaces;
using Timer = System.Timers.Timer;

namespace Othello.Controllers;

public class InputController : IConsoleInputController
{
    private const int MoveTimeoutInSeconds = 20;
    private readonly Timer _inputTimer;
    private CancellationTokenSource _cts;

    public InputController()
    {
        _inputTimer = new Timer(MoveTimeoutInSeconds * 1000); // Set the timer for 20 seconds
        _inputTimer.Elapsed += (_, _) => OnTimerElapsed();
        _cts = new CancellationTokenSource();
        _inputTimer.AutoReset = false; // Run timer once per input
    }

    public (int, int) GetMoveInput()
    {
        _inputTimer.Start(); // Start the timer

        try
        {
            while (true)
            {
                if (_cts.Token.IsCancellationRequested)
                    // Handle the timeout condition here, since exception won't work
                    throw new MoveTimeoutException("Move input timeout occurred.");

                if (Console.KeyAvailable)
                {
                    var input = Console.ReadLine();
                    if (input?.ToLower() == "hints")
                    {
                        _inputTimer.Stop();
                        throw new HintRequestedException("Hint requested by the user.");
                    }

                    if (input?.ToLower() == "undo")
                    {
                        _inputTimer.Stop();
                        throw new UndoRequestedException("Hint requested by the user.");
                    }

                    var parts = input?.Split();
                    if (parts is {Length: 2}
                        && int.TryParse(parts[0], out var row)
                        && int.TryParse(parts[1], out var col))
                    {
                        _inputTimer.Stop(); // Valid move entered, stop the timer
                        return (row, col);
                    }
                }

                Thread.Sleep(100); // Reduce CPU usage
            }
        }
        finally
        {
            _cts.Dispose(); // Clean up the CancellationTokenSource
            _cts = new CancellationTokenSource(); // Reset for next input
            _inputTimer.Start(); // Restart the timer for the next input
        }
    }

    public string GetGameModeInput()
    {
        Console.WriteLine("Select game mode: \n1. Player vs Player (PvP)\n2. Player vs Bot (PvE)");
        while (true)
        {
            var input = Console.ReadLine();
            if (input == "1" || input == "2") return input;

            Console.WriteLine("Invalid input, please select 1 or 2.");
        }
    }

    public bool AskPlayAgain()
    {
        Console.WriteLine("Do you want to play another game? (yes/no)");
        while (true)
        {
            var response = Console.ReadLine();
            if (response == null)
                Console.WriteLine("Invalid input, please select from (yes/no)");
            else
                return response.Trim().StartsWith("y", StringComparison.CurrentCultureIgnoreCase);
        }
    }

    private void OnTimerElapsed()
    {
        _inputTimer.Stop(); // Ensure the timer is stopped
        _cts.Cancel(); // Signal the cancellation
    }

    public class MoveTimeoutException(string message) : Exception(message);

    public class HintRequestedException(string message) : Exception(message);

    public class UndoRequestedException(string message) : Exception(message);
}