namespace Othello.Models
{
    public enum CellState
    {
        Empty,
        Black,
        White
    }

    public class Board
    {
        public CellState[,] Cells { get; private set; }
        public int Size { get; private set; }

        public Board(int size = 8)
        {
            if (size % 2 != 0 || size < 4)
            {
                throw new ArgumentException("Size must be an even number and at least 4.", nameof(size));
            }

            Size = size;
            Cells = new CellState[size, size];
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cells[i, j] = CellState.Empty;
                }
            }

            // Set up the initial four pieces in the center
            int midPoint = Size / 2;
            Cells[midPoint - 1, midPoint - 1] = CellState.Black;
            Cells[midPoint, midPoint] = CellState.Black;
            Cells[midPoint - 1, midPoint] = CellState.White;
            Cells[midPoint, midPoint - 1] = CellState.White;
        }

        private readonly (int, int)[] _directions =
        {
            (-1, -1), (-1, 0), (-1, 1), // Upper row
            (0, -1), /*(0, 0),*/ (0, 1), // Middle row
            (1, -1), (1, 0), (1, 1) // Lower row
        };

        public void MakeMove(int x, int y, CellState playerColor)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
            {
                throw new ArgumentOutOfRangeException("Move is outside the board boundaries.");
            }

            if (Cells[x, y] != CellState.Empty)
            {
                throw new InvalidOperationException("Cell is already occupied.");
            }

            if (playerColor == CellState.Empty)
            {
                throw new ArgumentException("Invalid player color.");
            }

            if (!IsValidMove(x, y, playerColor))
            {
                throw new InvalidOperationException("This move is invalid.");
            }

            Cells[x, y] = playerColor; // Place the player's piece on the board
            FlipPieces(x, y, playerColor);
        }

        public bool IsValidMove(int x, int y, CellState playerColor)
        {
            if (Cells[x, y] != CellState.Empty) return false;

            CellState opponentColor = playerColor == CellState.Black ? CellState.White : CellState.Black;

            foreach (var (dx, dy) in _directions)
            {
                int currentX = x + dx;
                int currentY = y + dy;

                bool hasOpponentBetween = false;

                while (currentX >= 0 && currentX < Size && currentY >= 0 && currentY < Size)
                {
                    if (Cells[currentX, currentY] == opponentColor)
                    {
                        hasOpponentBetween = true;
                    }
                    else if (Cells[currentX, currentY] == playerColor && hasOpponentBetween)
                    {
                        return true; // Valid move found
                    }
                    else
                    {
                        break; // Empty cell or no bracketing piece
                    }

                    currentX += dx;
                    currentY += dy;
                }
            }

            return false; // No valid moves found
        }

        public void FlipPieces(int x, int y, CellState playerColor)
        {
            CellState opponentColor = playerColor == CellState.Black ? CellState.White : CellState.Black;

            foreach (var (dx, dy) in _directions)
            {
                List<(int, int)> piecesToFlip = new List<(int, int)>();
                int currentX = x + dx;
                int currentY = y + dy;

                while (currentX >= 0 && currentX < Size && currentY >= 0 && currentY < Size)
                {
                    if (Cells[currentX, currentY] == opponentColor)
                    {
                        piecesToFlip.Add((currentX, currentY));
                    }
                    else if (Cells[currentX, currentY] == playerColor)
                    {
                        foreach (var (flipX, flipY) in piecesToFlip)
                        {
                            Cells[flipX, flipY] = playerColor; // Flip the piece
                        }

                        break; // Completed flipping for this direction
                    }
                    else
                    {
                        break; // Empty cell or same color, stop checking this direction
                    }

                    currentX += dx;
                    currentY += dy;
                }
            }
        }

        public List<(int, int)>? GetAvailableMoves(CellState playerColor)
        {
            List<(int, int)>? validMoves = new List<(int, int)>();
        
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (IsValidMove(row, col, playerColor))
                    {
                        validMoves.Add((row, col));
                    }
                }
            }

            return validMoves;
        }

        public List<(int, int)> CalculateFlips(int row, int col, CellState playerColor)
        {
            List<(int, int)> flippedPieces = new List<(int, int)>();
            CellState opponentColor = playerColor == CellState.Black ? CellState.White : CellState.Black;

            foreach (var (dx, dy) in _directions)
            {
                int currentX = row + dx;
                int currentY = col + dy;
                List<(int, int)> piecesToFlip = new List<(int, int)>();

                while (currentX >= 0 && currentX < Size && currentY >= 0 && currentY < Size
                       && Cells[currentX, currentY] == opponentColor)
                {
                    piecesToFlip.Add((currentX, currentY));
                    currentX += dx;
                    currentY += dy;
                }

                // Check if at the end of the line there is a piece of the current player's color
                if (currentX >= 0 && currentX < Size && currentY >= 0 && currentY < Size
                    && Cells[currentX, currentY] == playerColor)
                {
                    flippedPieces.AddRange(piecesToFlip);
                }
            }

            return flippedPieces;
        }

        public void UndoMove(Move move)
        {
            // Revert the last move using the information stored in the Move object
            // This includes setting the last moved position back to its original state and flipping back the flipped pieces
            Cells[move.Row, move.Col] =
                CellState.Empty; // Assuming the original state is always empty, adjust if necessary

            // Revert flips
            foreach (var (flipRow, flipCol) in move.FlippedPieces)
            {
                Cells[flipRow, flipCol] = move.PlayerColor == CellState.Black ? CellState.White : CellState.Black;
            }
        }
    }
}