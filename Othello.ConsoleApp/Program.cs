using Othello.Controllers;
using Othello.Controllers.Interfaces;
using Othello.Models;
using Othello.Views;

var playAgain = true;

while (playAgain)
{
    // Instantiate the input controller
    IConsoleInputController inputController = new InputController();

    // Game mode selection
    var modeInput = inputController.GetGameModeInput();
    Player player1 = new HumanPlayer(CellState.Black);
    Player player2;

    if (modeInput == "1")
        // Player vs Player
        player2 = new HumanPlayer(CellState.White);
    else
        // Player vs Bot
        player2 = new AIBot(CellState.White);

    // Create and register the view
    var view = new ConsoleView();

    // Instantiate the game model
    var game = new Game(player1, player2, view);

    // Create the game controller
    var gameController = new GameController(game, inputController);

    // Start the game loop
    gameController.StartGame();

    // Ask if the user wants to play again
    playAgain = inputController.AskPlayAgain();
}