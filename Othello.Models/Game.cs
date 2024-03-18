using Othello.Models.Interfaces;

namespace Othello.Models;

public class Game
{
    public Board Board { get; private set; }
    public Player CurrentPlayer { get; private set; }
    private Player OpponentPlayer { get; set; }
    public bool IsGameOver { get; private set; }
    private Player Winner { get; set; }

    private readonly IGameViewUpdater _observer;

    private readonly Stack<Move> _moveHistory = new();
    public Dictionary<CellState, int> Score => CalculateScore();

    public Game(Player player1, Player player2, IGameViewUpdater observer)
    {
        Board = new Board();
        CurrentPlayer = player1;
        OpponentPlayer = player2;
        _observer = observer;
    }

    public void Start()
    {
        IsGameOver = false;
        Winner = null!;
        CurrentPlayer = DecideStartingPlayer();
        NotifyObservers("Game has started. Board is initialized.");
        UpdateBoardView();
        NotifyObservers($"Player {CurrentPlayer.Color}'s turn. Please enter your move (row col):");
    }

    public bool MakeMove(int row, int col)
    {
        if (row < 0 || row > 7 || col < 0 || col > 7)
        {
            NotifyObservers("Move is outside the board boundaries");
            return false;
        }

        if (!Board.IsValidMove(row, col, CurrentPlayer.Color))
        {
            NotifyObservers("Invalid move, try again");
            return false;
        }

        // Before making the move on the board, calculate flipped pieces and store the move
        var flippedPieces = Board.CalculateFlips(row, col, CurrentPlayer.Color);
        _moveHistory.Push(new Move(row, col, CurrentPlayer.Color, flippedPieces));

        Board.MakeMove(row, col, CurrentPlayer.Color);
        UpdateBoardView();
        SwitchTurns();
        return true;
    }

    private void SwitchTurns()
    {
        (CurrentPlayer, OpponentPlayer) = (OpponentPlayer, CurrentPlayer);
        if (!PlayerCanMove(CurrentPlayer))
        {
            NotifyObservers($"There is no move available for {CurrentPlayer} \n {CurrentPlayer} passes");
            (CurrentPlayer, OpponentPlayer) = (OpponentPlayer, CurrentPlayer);
        }

        NotifyPlayerTurn(CurrentPlayer);
    }

    private Dictionary<CellState, int> CalculateScore()
    {
        var score = new Dictionary<CellState, int>
        {
            {CellState.Black, 0},
            {CellState.White, 0}
        };

        for (var row = 0; row < Board.Size; row++)
        for (var col = 0; col < Board.Size; col++)
            if (Board.Cells[row, col] == CellState.Black)
                score[CellState.Black]++;
            else if (Board.Cells[row, col] == CellState.White)
                score[CellState.White]++;

        return score;
    }

    private bool PlayerCanMove(Player player)
    {
        for (var row = 0; row < Board.Size; row++)
        for (var col = 0; col < Board.Size; col++)
            if (Board.IsValidMove(row, col, player.Color))
                return true;

        return false;
    }

    public bool CheckGameOver()
    {
        // The game is over if neither player can make a valid move
        var currentPlayerCanMove = PlayerCanMove(CurrentPlayer);
        var opponentPlayerCanMove = PlayerCanMove(OpponentPlayer);

        if (!currentPlayerCanMove && !opponentPlayerCanMove)
            IsGameOver = true;
        else
            IsGameOver = false;

        return IsGameOver;
    }

    public void EndGame()
    {
        IsGameOver = true;
        DetermineWinner(); // Determine the winner before constructing the game over message

        var finalScore = CalculateScore();
        var gameOverMessage =
            $"Game Over\nScore - Black: {finalScore[CellState.Black]}, White: {finalScore[CellState.White]}";

        if (Winner != null)
            gameOverMessage += $"\nWinner is {Winner.Color}";
        else
            gameOverMessage += "\nThe game is a tie.";

        NotifyObservers(gameOverMessage);
    }


    private Player DecideStartingPlayer()
    {
        // Black always starts first
        return CurrentPlayer.Color == CellState.Black ? CurrentPlayer : OpponentPlayer;
    }

    private void NotifyObservers(string message)
    {
        _observer.Update(message);
    }

    private void UpdateBoardView()
    {
        var boardState = Board.Cells; // Assuming Board.Cells is accessible
        _observer.DisplayBoard(boardState, null);
    }

    private void NotifyPlayerTurn(Player player)
    {
        if (player is HumanPlayer)
            NotifyObservers($"Player {player.Color}'s turn. Please enter your move (row col):");
        else if (player is AIBot)
            // No need for input prompt message for AI Bot. Optionally, notify about AI thinking.
            NotifyObservers($"AI Bot {player.Color}'s turn...");
    }

    private void DetermineWinner()
    {
        var finalScore = CalculateScore();
        if (finalScore[CellState.Black] > finalScore[CellState.White])
            Winner = CurrentPlayer.Color == CellState.Black ? CurrentPlayer : OpponentPlayer;
        else if (finalScore[CellState.White] > finalScore[CellState.Black])
            Winner = CurrentPlayer.Color == CellState.White ? CurrentPlayer : OpponentPlayer;
        else
            // It's a tie if scores are equal
            Winner = null;
    }

    public void ShowHints()
    {
        var hints = Board.GetAvailableMoves(CurrentPlayer.Color);
        _observer.DisplayBoard(Board.Cells, hints);
    }

    public void PerformRandomMove()
    {
        try
        {
            var bot = new AIBot(CurrentPlayer.Color);
            var move = bot.MakeMove(Board);
            MakeMove(move.Item1, move.Item2);
            UpdateBoardView();
            NotifyObservers($"Random move {move.Item1 + 1} {move.Item2 + 1} was made due to timeout");
        }
        catch (InvalidOperationException)
        {
        }
    }

    public void UndoMove()
    {
        if (_moveHistory.Any())
        {
            var lastMove = _moveHistory.Peek(); // Look at the last move without popping it
            var timeSinceLastMove = DateTime.Now - lastMove.MoveTime;
            if (timeSinceLastMove <= TimeSpan.FromSeconds(3))
            {
                // If within 3 seconds, pop the move and undo it
                _moveHistory.Pop();
                Board.UndoMove(lastMove);

                // Switch turns back to the player who made the move
                SwitchTurns();

                NotifyObservers($"Undo successful. It's now {CurrentPlayer.Color}'s turn again.");
                UpdateBoardView();
            }
            else
            {
                // If more than 3 seconds have passed, do not allow the undo
                NotifyObservers("Undo not allowed. More than 3 seconds have passed since the last move.");
            }
        }
    }
}