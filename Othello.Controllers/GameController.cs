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
            // Display the initial state of the board.
            // This can be done by calling a method on the view directly or through the game notifying its observers.

            while (!_game.IsGameOver)
            {
                // _game.UpdateBoardView(); // Update the board view at the start of each turn.
                var currentPlayer = _game.CurrentPlayer;
        
                Console.WriteLine($"Player {currentPlayer.Color}'s turn. Please enter your move (row col):");
                var move = _inputController.GetMoveInput();

                if (_game.MakeMove(move.Item2 - 1, move.Item1 - 1))
                {
                    // Move was successful, check for game over or switch turns.
                    if (_game.CheckGameOver()) // This method needs to be implemented.
                    {
                        _game.EndGame();
                        return;
                    }
                }
                else
                {
                    // Invalid move, the notification is handled within the Game class.
                    continue; // Prompt the same player to enter a valid move.
                }
            }
        }
    }
}