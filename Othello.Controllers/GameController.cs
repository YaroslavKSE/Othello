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

        public void StartGame()
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
                            SimulateAiDelay();
                            var (row, col) = currentPlayer.MakeMove(_game.Board);
                            _game.MakeMove(row, col);
                            break;
                    }
                }
                catch (InputController.MoveTimeoutException)
                {
                    _game.PerformRandomMove();
                }
                catch (InputController.HintRequestedException)
                {
                    // Handle hint request (show hint or make a hint move)
                    _game.ShowHints();
                }

                if (_game.CheckGameOver())
                {
                    _game.EndGame();
                    Console.WriteLine("Game Over");
                    // Optionally, display the score or winner here
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
    }
}