using Othello.Controllers.Interfaces;
using Othello.Models;

namespace Othello.Controllers
{
    public class GameController : IGameController
    {
        private readonly Game _game;
        private readonly IConsoleInputController _inputController;
        private CancellationTokenSource _undoCancellationTokenSource = null!;

        public GameController(Game game, IConsoleInputController inputController)
        {
            _game = game;
            _inputController = inputController;
        }

        public async Task StartGame()
        {
            _game.Start();

            while (!_game.IsGameOver)
            {
                var currentPlayer = _game.CurrentPlayer;
                switch (currentPlayer)
                {
                    // Check the type of the current player to decide on the move source
                    case HumanPlayer:
                        var move = _inputController.GetMoveInput();
                        _game.MakeMove(move.Item1 - 1, move.Item2 - 1);
                        _undoCancellationTokenSource = new CancellationTokenSource();

                        // Start listening for undo in a non-blocking way
                        var undoTask = WaitForUndoRequest(_undoCancellationTokenSource.Token);

                        // If undo was requested and no move was made by the opponent, undo the last move
                        if (await Task.WhenAny(undoTask, Task.Delay(3000)) == undoTask && undoTask.Result)
                        {
                            _game.UndoMove();
                        }

                        await _undoCancellationTokenSource.CancelAsync(); // Cancel listening for undo requests

                        break;
                    case AIBot:
                        // For AIBot, the move is generated within the MakeMove method itself
                        SimulateAiDelay(); // Simulate AI thinking delay
                        var (row, col) = currentPlayer.MakeMove(_game.Board);
                        // Console.WriteLine($"AI Bot {currentPlayer.Color} makes a move: {row + 1} {col + 1}");
                        _game.MakeMove(row, col);
                        break;
                }

                if (_game.CheckGameOver())
                {
                    _game.EndGame();
                    return;
                }
            }
        }

        public void SimulateAiDelay()
        {
            // Introduce a random delay between 1 and 3 seconds
            Random rand = new Random();
            int delay = rand.Next(1000, 3001); // Milliseconds
            // Console.WriteLine("AI is thinking...");
            Task.Delay(delay).Wait(); // Use await Task.Delay(delay) in async methods
        }

        public async Task<bool> WaitForUndoRequest(CancellationToken token)
        {
            // Listen for 'U' key press to undo
            while (!token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    if (key == ConsoleKey.U)
                    {
                        return true; // Undo requested
                    }
                }

                await Task.Delay(100, token); // Polling delay to reduce CPU usage
            }

            return false; // No undo requested or cancellation token triggered
        }
    }
}