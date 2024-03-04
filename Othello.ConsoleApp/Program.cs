// Step 1: Create players
using Othello.Controllers;
using Othello.Controllers.Interfaces;
using Othello.Models;
using Othello.Views;

Player player1 = new HumanPlayer(CellState.Black);
Player player2 = new AIBot(CellState.White); // Or another HumanPlayer for PvP

// Step 2: Instantiate the game model
Game game = new Game(player1, player2);

// Step 3: Create and register the view
ConsoleView view = new ConsoleView();
game.RegisterObserver(view);

// Step 4: Instantiate the input controller
IConsoleInputController inputController = new InputController();

// Step 5: Create the game controller
GameController gameController = new GameController(game, inputController);

// Optional: If you have specific setup or initialization for your view
// that depends on the initial state of the game (like displaying the initial board),
// you might call it here.
// view.DisplayBoard(game.Board.Cells);

// Step 6: Start the game loop
gameController.StartGame();
