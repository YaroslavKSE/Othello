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
                await currentPlayer.MakeMoveAsync(_game);
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
}