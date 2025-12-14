namespace ChameleonGame.Model
{
    public class ChameleonBoard
    {
        #region public properties
        public int Size { get; }
        public Cell[,] Board { get; private set; }

        public Cell this[int r, int c] => Board[r, c];
        #endregion

        private readonly List<Piece> colorChangeQueue = [];

        #region constructors and initialization
        public ChameleonBoard(int size)
        {
            if (size is not (3 or 5 or 7))
            {
                throw new ArgumentException("Board size must be 3 or 5 or 7.");
            }

            Size = size;
            Board = new Cell[size, size];

            InitializeBoard();
        }

        public ChameleonBoard(int size, List<Piece> pieces)
        {
            if (size is not (3 or 5 or 7))
            {
                throw new ArgumentException("Board size must be 3 or 5 or 7.");
            }

            Size = size;
            Board = new Cell[size, size];

            InitializeBoard();

            foreach (Piece piece in pieces)
            {
                Board[piece.Row, piece.Col].ChangePiece(piece);
            }
        }

        private void InitializeBoard()
        {
            int top = 0, bottom = Size - 2, left = 0, right = Size - 1;

            // Red cells
            while (left <= right && top <= bottom)
            {
                // from top-right to left
                for (int i = right; i >= left; i--)
                {
                    Board[top, i] = new Cell(top, i, CellColor.Red);
                }
                top++;
                right--;

                // from top-left downwards
                for (int i = top; i <= bottom; i++)
                {
                    Board[i, left] = new Cell(i, left, CellColor.Red);
                }
                left++;
                top++;

                if (left > right || top > bottom) break;

                // from down-left to right
                for (int i = left; i <= right; i++)
                {
                    Board[bottom, i] = new Cell(bottom, i, CellColor.Red);
                }
                bottom--;
                left++;

                // from down-right upwards
                for (int i = bottom; i >= top; i--)
                {
                    Board[i, right] = new Cell(i, right, CellColor.Red);
                }
                bottom--;
                right--;
            }

            // Green cells
            top = 1; bottom = Size - 1; left = 0; right = Size - 1;
            while (left <= right && top <= bottom)
            {
                // from down-left to right
                for (int i = left; i <= right; i++)
                {
                    Board[bottom, i] = new Cell(bottom, i, CellColor.Green);
                }
                bottom--;
                left++;

                // from down-right upwards
                for (int i = bottom; i >= top; i--)
                {
                    Board[i, right] = new Cell(i, right, CellColor.Green);
                }
                bottom--;
                right--;

                if (left > right || top > bottom) break;

                // from top-right to left
                for (int i = right; i >= left; i--)
                {
                    Board[top, i] = new Cell(top, i, CellColor.Green);
                }
                top++;
                right--;

                // from top-left downwards
                for (int i = top; i <= bottom; i++)
                {
                    Board[i, left] = new Cell(i, left, CellColor.Green);
                }
                left++;
                top++;
            }

            int middle = (Size / 2);
            Board[middle, middle] = new Cell(middle, middle, CellColor.Gray);
        }

        #endregion

        #region public methods
        // Only called when new game is created.
        public void InitializePieces()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Board[i, j].Color == CellColor.Red)
                    {
                        Board[i, j].ChangePiece(new Piece(Player.Red, i, j));
                    }
                    else if (Board[i, j].Color == CellColor.Green)
                    {
                        Board[i, j].ChangePiece(new Piece(Player.Green, i, j));
                    }
                }
            }
        }

        public bool ValidCell(Cell cell)
        {
            return cell.Row >= 0 && cell.Row < Size && cell.Col >= 0 && cell.Col < Size;
        }

        public bool TryMovePiece(Cell from, Cell to, Player currentPlayer)
        {
            if (!ValidCell(from) || !ValidCell(to) || from.Piece == null || to.Piece != null || from.Piece.Owner != currentPlayer)
            {
                return false;
            }

            int dr = Math.Abs(from.Row - to.Row);
            int dc = Math.Abs(from.Col - to.Col);

            // Vertical or Vertical check
            if (!(dr == 1 && dc == 0) && !(dr == 0 && dc == 1))
            {
                return false;
            }

            Piece piece = from.Piece;
            to.ChangePiece(piece);
            from.ChangePiece(null);
            piece.SetCurrentCell(to);

            HandleColorChange(piece, to);

            return true;
        }

        public void HandleColorChange(Piece piece, Cell to)
        {
            if (colorChangeQueue.Contains(piece))
            {
                if (to.Color == CellColor.Gray || (piece.Owner == Player.Red && to.Color == CellColor.Red) || (piece.Owner == Player.Green && to.Color == CellColor.Green))
                {
                    colorChangeQueue.Remove(piece);
                    piece.ResetDelay();
                    return;
                }
            }
            else
            {
                if (to.Color == CellColor.Gray) { return; }

                if ((piece.Owner == Player.Red && to.Color == CellColor.Green) || (piece.Owner == Player.Green && to.Color == CellColor.Red))
                {
                    colorChangeQueue.Add(piece);
                    return;
                }
            }
        }

        public bool TryJump(Cell from, Cell to, Player currentPlayer)
        {
            if (!ValidCell(from) || !ValidCell(to) || from.Piece == null || to.Piece != null || from.Piece.Owner != currentPlayer)
            {
                return false;
            }

            int dr = to.Row - from.Row;
            int dc = to.Col - from.Col;

            // Vertical and Horizontal check
            if (!(Math.Abs(dr) == 2 && Math.Abs(dc) == 0) && !(Math.Abs(dr) == 0 && Math.Abs(dc) == 2)) { return false; }

            int midr = from.Row + (dr / 2);
            int midc = from.Col + (dc / 2);

            Cell middleCell = Board[midr, midc];

            // Jumped over enemy check
            if (middleCell.Piece == null)
            {
                return false;
            }

            if (middleCell.Piece.Owner == currentPlayer)
            {
                return false;
            }

            middleCell.ChangePiece(null);

            Piece piece = from.Piece;
            to.ChangePiece(piece);
            from.ChangePiece(null);
            piece.SetCurrentCell(to);

            HandleColorChange(piece, to);

            return true;
        }

        public void PerformColorChange()
        {
            List<Piece> toRemove = [];
            foreach (Piece piece in colorChangeQueue)
            {
                if (piece.ColorChangeDelay == 2)    // increased once in current turn, once in opponent's turn, then color change happens
                {
                    piece.ChangeColor();
                    toRemove.Add(piece);
                    continue;
                }

                piece.IncrementDelay();
            }

            foreach (Piece piece in toRemove)
            {
                _ = colorChangeQueue.Remove(piece);
            }
        }

        public bool PlayerHasPieces(Player player)
        {
            return Board.Cast<Cell>().Any(c => c.Piece?.Owner == player);
        }
        #endregion
    }
}
