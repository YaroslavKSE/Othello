using Othello.Models;

namespace Othello.Tests;

public class FlipPiecesUnitTests
{
    [Fact]
    public void FlipPieces_ValidMove_FlipsCorrectPieces()
    {
        // Arrange
        var board = new Board();
        var playerColor = CellState.Black;

        // Act
        board.FlipPieces(2, 4, playerColor);  // Assuming this is a valid move that should flip at least one piece

        // Assert
        Assert.Equal(playerColor, board.Cells[3, 4]); // The piece that should be flipped
    }
}