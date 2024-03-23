using Othello.Controllers;
using Othello.Controllers.Interfaces;
using Othello.Models;
using Othello.Models.Interfaces;
using Othello.Views;

var playAgain = true;

while (playAgain)
{
    // Instantiate the input controller
    IConsoleInputController inputController = new InputController();
    IPlayerInputGetter inputGetter = new InputController();
    IUndoRequestListener undoRequestListener = new InputController();
    // Game mode selection
    var modeInput = inputController.GetGameModeInput();
    Player player1 = new HumanPlayer(CellState.Black, inputGetter);
    Player player2;

    if (modeInput == "1")
        // Player vs Player
        player2 = new HumanPlayer(CellState.White, inputGetter);
    else
        // Player vs Bot
        player2 = new AIBot(CellState.White, undoRequestListener);

    // Create and register the view
    var view = new ConsoleView();

    // Instantiate the game model
    var game = new Game(player1, player2, view);

    // Create the game controller
    var gameController = new GameController(game, inputController);

    // Start the game loop
    await gameController.StartGame();

    // Ask if the user wants to play again
    playAgain = inputController.AskPlayAgain();
}