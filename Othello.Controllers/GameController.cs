using Othello.Controllers.Interfaces;
using Othello.Models;

namespace Othello.Controllers;

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

        while (!_game.IsGameOver)
        {
            var currentPlayer = _game.CurrentPlayer;

            try
            {
                switch (currentPlayer)
                {
                    case HumanPlayer:
                        var move = _inputController.GetMoveInput();
                        _game.MakeMove(move.Item1 - 1, move.Item2 - 1);
                        break;
                    case AIBot:
                        // Start AI move in a separate task
                        var aiMoveTask = currentPlayer.MakeMoveAsync(_game.Board);
                        var undoRequested = false;
                        while (!aiMoveTask.IsCompleted)
                        {
                            if (Console.KeyAvailable)
                            {
                                var key = Console.ReadKey(intercept: true);
                                if (key.Key == ConsoleKey.U) // Assuming 'U' is the undo command
                                {
                                    undoRequested = true;
                                    _game.UndoMove();
                                }
                            }

                            // Sleep to reduce CPU usage
                            await Task.Delay(100);
                        }

                        if (undoRequested == false)
                        {                        
                            await aiMoveTask; // Ensure AI move is completed
                            var (row, col) = aiMoveTask.Result;
                            _game.MakeMove(row, col);
                            
                        }
                        break;
                }
            }
            catch (InputController.MoveTimeoutException)
            {
                _game.PerformRandomMove();
            }
            catch (InputController.HintRequestedException)
            {
                _game.ShowHints();
            }
            catch (InputController.UndoRequestedException)
            {
                _game.UndoMove();
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
        var rand = new Random();
        var delay = rand.Next(1000, 3001); // Milliseconds
        Task.Delay(delay);
    }
}