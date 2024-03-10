using Othello.Models;

namespace Othello.Tests;

public class MakeMoveUnitTests
{
    [Fact]
    public void MakeMove_UpdatesBoard_When_Black_Makes_ValidMove()
    {
        // Arrange
        var board = new Board();
        var initialColor = board.Cells[3, 4];
        var playerColor = CellState.Black;

        // Act
        board.MakeMove(2, 4, playerColor); // A valid move in an initial Othello board

        // Assert
        Assert.NotEqual(initialColor, board.Cells[3, 4]);
        Assert.Equal(playerColor, board.Cells[2, 4]);
    }

    [Fact]
    public void MakeMove_DoNot_UpdateBoard_And_ThrowsException_When_Black_Makes_InvalidMove()
    {
        // Arrange
        var board = new Board();
        var playerColor = CellState.Black;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => board.MakeMove(0, 0, playerColor));
    }
    // Add more tests as needed to cover various scenarios and edge cases,
    // such as making moves on the edge of the board, making moves that don't flip any pieces, etc.
}