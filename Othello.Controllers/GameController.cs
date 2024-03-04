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
                var move = _inputController.GetMoveInput();

                // MakeMove internally checks for validity and notifies observers if invalid.
                var moveMade = _game.MakeMove(move.Item1, move.Item2);
                if (!moveMade)
                {
                    // The notification for invalid move is handled within the Game class.
                    continue;
                }

                // Any additional logic needed after a successful move.
                
            }
        }
    }
}