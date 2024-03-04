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
            // Board.InitializeBoard(); // Ensure the board is in starting configuration
            CurrentPlayer = DecideStartingPlayer();

            while (!IsGameOver)
            {
                if (!PlayerCanMove(CurrentPlayer) && !PlayerCanMove(OpponentPlayer))
                {
                    EndGame();
                    break;
                }

                if (!PlayerCanMove(CurrentPlayer))
                {
                    Console.WriteLine($"Player {CurrentPlayer.Color} has no valid moves and will be skipped.");
                    SwitchTurns();
                    continue;
                }
                // Here, you would typically prompt the current player to make a move,
                // which might involve updating the game board and then switching turns.
                // This loop structure is just a conceptual demonstration.
            }
        }

        public bool MakeMove(int row, int col)
        {
            if (!Board.IsValidMove(row, col, CurrentPlayer.Color))
            {
                NotifyObservers("Invalid move, try again");
                return false;
            }
            
            Board.MakeMove(row, col, CurrentPlayer.Color);
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

        private void EndGame()
        {
            IsGameOver = true;
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

        protected void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }
}