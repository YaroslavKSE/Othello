using System.Diagnostics;
using Othello.Controllers.Interfaces;
using Othello.Models;

namespace Othello.Controllers
{
    public class GameController : IGameController
    {
        private readonly Game _game;
        private readonly IConsoleInputController _inputController;

        public GameController(Game game, IConsoleInputController inputController)
        {
            _game = game;
            _inputController = inputController;
        }

        public async Task StartGame()
        {
            _game.Start();
            Stopwatch stopwatch = new Stopwatch();

            while (!_game.IsGameOver)
            {
                var currentPlayer = _game.CurrentPlayer;
                stopwatch.Restart();
                // After making a move (either by player or AI)
                switch (currentPlayer)
                {
                    // Check the type of the current player to decide on the move source
                    case HumanPlayer:
                        var move = _inputController.GetMoveInput();
                        _game.MakeMove(move.Item1 - 1, move.Item2 - 1);
                        stopwatch.Restart();
                        if (stopwatch.ElapsedMilliseconds <= 3000)
                        {
                            Console.WriteLine("Move made. You have got 3 seconds to undo. \nPress 'U' to undo.");
                            bool undoRequested = await WaitForUndoRequest(3000);
                            if (undoRequested)
                            {
                                _game.UndoMove();
                            }
                        }

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

        public async Task<bool> WaitForUndoRequest(int timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            var undoTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.U) // Let 'U' be the undo command
                        {
                            return true;
                        }
                    }
                }

                return false;
            }, token);

            // Delay for the specified timeout
            await Task.Delay(timeout, token).ContinueWith(t => { }, token);

            // If we reach here, the delay has completed. Cancel the undoTask.
            await cancellationTokenSource.CancelAsync();

            try
            {
                // If the task completed with a result before cancellation, this will return the result.
                return await undoTask;
            }
            catch (TaskCanceledException)
            {
                // If the task was cancelled, treat it as no undo request.
                return false;
            }
        }
    }
}