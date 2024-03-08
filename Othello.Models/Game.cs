using Othello.Models.Interfaces;

namespace Othello.Models
{
    public class Game
    {
        public Board Board { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public Player OpponentPlayer { get; private set; }
        public bool IsGameOver { get; private set; }
        public Player Winner { get; private set; }
        
        private readonly List<IGameViewUpdater> _observers = new();


        public Dictionary<CellState, int> Score => CalculateScore();

        public Game(Player player1, Player player2)
        {
            Board = new Board();
            CurrentPlayer = player1;
            OpponentPlayer = player2;
        }

        public void Start()
        {
            IsGameOver = false;
            Winner = null;
            CurrentPlayer = DecideStartingPlayer();
            // Optionally, notify the observers (view) to display the initial game state.
            NotifyObservers("Game has started. Board is initialized.");
            UpdateBoardView(); // Assuming this method notifies the view to display the board.
        }

        public bool MakeMove(int row, int col)
        {
            if (!Board.IsValidMove(row, col, CurrentPlayer.Color))
            {
                NotifyObservers("Invalid move, try again");
                return false;
            }
            
            Board.MakeMove(row, col, CurrentPlayer.Color);
            UpdateBoardView();
            // Board.FlipPieces(row, col, CurrentPlayer.Color);
            SwitchTurns();
            return true;
        }

        private void SwitchTurns()
        {
            (CurrentPlayer, OpponentPlayer) = (OpponentPlayer, CurrentPlayer);
        }

        private Dictionary<CellState, int> CalculateScore()
        {
            var score = new Dictionary<CellState, int>
            {
                {CellState.Black, 0},
                {CellState.White, 0}
            };

            for (int row = 0; row < Board.Size; row++)
            {
                for (int col = 0; col < Board.Size; col++)
                {
                    if (Board.Cells[row, col] == CellState.Black)
                        score[CellState.Black]++;
                    else if (Board.Cells[row, col] == CellState.White)
                        score[CellState.White]++;
                }
            }

            return score;
        }

        private bool PlayerCanMove(Player player)
        {
            for (int row = 0; row < Board.Size; row++)
            {
                for (int col = 0; col < Board.Size; col++)
                {
                    if (Board.IsValidMove(row, col, player.Color))
                        return true;
                }
            }

            return false;
        }
        public bool CheckGameOver()
        {
            // The game is over if neither player can make a valid move
            bool currentPlayerCanMove = PlayerCanMove(CurrentPlayer);
            bool opponentPlayerCanMove = PlayerCanMove(OpponentPlayer);

            if (!currentPlayerCanMove && !opponentPlayerCanMove)
            {
                IsGameOver = true;
            }
            else
            {
                IsGameOver = false;
            }

            return IsGameOver;
        }
        
        public void EndGame()
        {
            IsGameOver = true;
            DetermineWinner(); // Determine the winner before constructing the game over message
    
            var finalScore = CalculateScore();
            var gameOverMessage = $"Game Over\nScore - Black: {finalScore[CellState.Black]}, White: {finalScore[CellState.White]}";

            if (Winner != null)
            {
                gameOverMessage += $"\nWinner is {Winner.Color}";
            }
            else
            {
                gameOverMessage += "\nThe game is a tie.";
            }

            NotifyObservers(gameOverMessage);
        }


        private Player DecideStartingPlayer()
        {
            // Black always starts first
            return CurrentPlayer.Color == CellState.Black ? CurrentPlayer : OpponentPlayer;
        }
        
        public void RegisterObserver(IGameViewUpdater observer)
        {
            _observers.Add(observer);
        }

        public void DeregisterObserver(IGameViewUpdater observer)
        {
            _observers.Remove(observer);
        }
        // Rewrite to one IGameViewUpdater
        
        protected void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
        public void UpdateBoardView()
        {
            var boardState = Board.Cells; // Assuming Board.Cells is accessible
            foreach (var observer in _observers)
            {
                if (observer is { } consoleView)
                {
                    consoleView.DisplayBoard(boardState);
                }
            }
        }
        private void DetermineWinner()
        {
            var finalScore = CalculateScore();
            if (finalScore[CellState.Black] > finalScore[CellState.White])
            {
                Winner = CurrentPlayer.Color == CellState.Black ? CurrentPlayer : OpponentPlayer;
            }
            else if (finalScore[CellState.White] > finalScore[CellState.Black])
            {
                Winner = CurrentPlayer.Color == CellState.White ? CurrentPlayer : OpponentPlayer;
            }
            else
            {
                // It's a tie if scores are equal
                Winner = null;
            }
        }


    }
}