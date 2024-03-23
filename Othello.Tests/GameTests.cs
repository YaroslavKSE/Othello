using Moq;
using Othello.Controllers;
using Othello.Models;
using Othello.Models.Interfaces;


namespace Othello.Tests;

public class GameTests
{
    [Fact]
    public void Start_InitializesGameCorrectly()
    {
        var mockObserver = new Mock<IGameViewUpdater>();
        var inputGetter = new InputController();
        var player1 = new HumanPlayer(CellState.Black, inputGetter);
        var player2 = new HumanPlayer(CellState.White, inputGetter);

        var game = new Game(player1, player2, mockObserver.Object);

        game.Start();

        mockObserver.Verify(x => x.Update(It.IsAny<string>()), Times.AtLeastOnce());
        Assert.False(game.IsGameOver);
        Assert.NotNull(game.CurrentPlayer);
    }

    [Fact]
    public void MakeMove_ValidMove_ReturnsTrue()
    {
        var mockObserver = new Mock<IGameViewUpdater>();
        var inputGetter = new InputController();
        var player1 = new HumanPlayer(CellState.Black, inputGetter);
        var player2 = new HumanPlayer(CellState.White, inputGetter);
        var game = new Game(player1, player2, mockObserver.Object);

        game.Start();
        // Assuming the starting player has a valid move at (3, 3) - adjust according to your Board's implementation
        var result = game.MakeMove(4, 2);

        Assert.True(result);
        mockObserver.Verify(x => x.DisplayBoard(It.IsAny<CellState[,]>(), null), Times.AtLeastOnce());
    }

    [Fact]
    public void UndoMove_WithinAllowedTime_PerformsUndo()
    {
        var mockObserver = new Mock<IGameViewUpdater>();
        // Setup your players and game instance
        var inputGetter = new InputController();
        var player1 = new HumanPlayer(CellState.Black, inputGetter);
        var player2 = new HumanPlayer(CellState.White, inputGetter);
        var game = new Game(player1, player2, mockObserver.Object);

        game.Start();
        // Make a move to have something to undo
        game.MakeMove(4, 2);

        // Simulate undoing the move quickly enough
        game.UndoMove();

        mockObserver.Verify(x => x.Update("Undo successful. It's now Black's turn again."), Times.Once);
    }


    [Fact]
    public void CheckGameOver_WhenGameIsStarted_ReturnsFalse()
    {
        var mockObserver = new Mock<IGameViewUpdater>();
        // Setup your players and game instance
        var inputGetter = new InputController();
        var player1 = new HumanPlayer(CellState.Black, inputGetter);
        var player2 = new HumanPlayer(CellState.White, inputGetter);
        var game = new Game(player1, player2, mockObserver.Object);

        // You might need to manipulate
        game.Start();

        var isGameOver = game.CheckGameOver();

        Assert.False(isGameOver);
    }

    [Fact]
    public void CalculateScore_AtGameStart_ReturnsCorrectInitialScores()
    {
        // Arrange
        var mockObserver = new Mock<IGameViewUpdater>();
        var inputGetter = new InputController();
        var player1 = new HumanPlayer(CellState.Black, inputGetter);
        var player2 = new HumanPlayer(CellState.White, inputGetter);
        var game = new Game(player1, player2, mockObserver.Object);

        // The initial setup is part of the Game constructor which includes the Board initialization
        // and thus setting up the initial four pieces in the center.

        // Act
        var score = game.CalculateScore(); // Score is a public getter that internally calls CalculateScore

        // Assert
        Assert.NotNull(score);
        Assert.Equal(2, score[CellState.Black]);
        Assert.Equal(2, score[CellState.White]);
    }

    [Fact]
    public void ShowHints_DisplaysAvailableMoves()
    {
        // Arrange
        var mockObserver = new Mock<IGameViewUpdater>();
        var inputGetter = new InputController();
        var player1 = new HumanPlayer(CellState.Black, inputGetter);
        var player2 = new HumanPlayer(CellState.White, inputGetter);
        var game = new Game(player1, player2, mockObserver.Object);

        // Mock the expected behavior if necessary. For example, if your game setup depends on it.
        // This might include setting up the board or manipulating the current player in a specific way if needed.

        // Act
        game.ShowHints();

        // Assert
        mockObserver.Verify(x => x.DisplayBoard(It.IsAny<CellState[,]>(), It.IsAny<List<(int, int)>>()), Times.Once);
    }

    [Fact]
    public void PerformRandomMove_MakesAMoveAndNotifies()
    {
        // Arrange
        var mockObserver = new Mock<IGameViewUpdater>();
        var inputGetter = new InputController();
        var player1 = new HumanPlayer(CellState.Black, inputGetter);
        var player2 = new HumanPlayer(CellState.White, inputGetter);
        var game = new Game(player1, player2, mockObserver.Object);

        // Act
        game.PerformRandomMove();

        // Assert
        // Verify that a move was made. 
        mockObserver.Verify(x => x.Update(It.Is<string>(s => s.Contains("Random move"))), Times.Once);
    }

    // Add more tests as needed...
}