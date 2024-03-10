using Othello.Models;

namespace Othello.Tests;

public class IsValidMoveUnitTests
{
    [Fact]
    public void IsValidMoveReturnsTrueIfBlackGoes_X2_Y4_When_GameIsStarted()
    {
        var board = new Board();
        var result = board.IsValidMove(2, 4, CellState.Black);
        Assert.True(result, "This is a valid move for Black because White [3, 4] is between Blacks");
    }

    [Fact]
    public void IsValidMoveReturnsTrueIfBlackGoes_X3_Y5_When_GameIsStarted()
    {
        var board = new Board();
        var result = board.IsValidMove(3, 5, CellState.Black);
        Assert.True(result, "This is a valid move for Black if because White [3, 4] is between Blacks");
    }

    [Fact]
    public void IsValidMoveReturnsFalseIfBlackGoes_X2_Y3_When_GameIsStarted()
    {
        var board = new Board();
        var result = board.IsValidMove(2, 3, CellState.Black);
        Assert.False(result, "This is a not valid move for Black because White[3, 4] is not bracketed");
    }

    [Fact]
    public void IsValidMoveReturnsFalseIfBlackGoes_X0_Y0_When_GameIsStarted()
    {
        var board = new Board();
        var result = board.IsValidMove(0, 0, CellState.Black);
        Assert.False(result, "This is a not valid move for Black because White[3, 4] is not bracketed");
    }

    [Fact]
    public void IsValidMoveReturnsTrueIfWhiteGoes_X4_Y5_When_GameIsStarted()
    {
        var board = new Board();
        var result = board.IsValidMove(4, 5, CellState.White);
        Assert.True(result, "This is a valid move for White because Black[4, 4] is bracketed");
    }
}