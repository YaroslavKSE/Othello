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

                switch (currentPlayer)
                {
                    // Check the type of the current player to decide on the move source
                    case HumanPlayer:
                        var move = _inputController.GetMoveInput();
                        _game.MakeMove(move.Item1 - 1, move.Item2 - 1);
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
    }
}