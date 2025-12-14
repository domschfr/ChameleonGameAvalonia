using ChameleonGame.Persistance;

namespace ChameleonGame.Model
{
    public enum Player
    {
        Red,
        Green
    }

    public class GameModel
    {
        #region private fields
        private IChameleonDataAccess? _dataAccess;
        private ChameleonBoard? _board = null;
        private Player _currentPlayer;
        private bool isGameOver = false;
        private (int r, int c)? _selectedCell = null;
        #endregion

        #region public properties
        public Player? Winner { get; private set; } = null;
        public IChameleonDataAccess? DataAccess { get; set; }
        public ChameleonBoard? Board { get => _board; }
        public Player CurrentPlayer { get => _currentPlayer; }
        public (int r, int c)? SelectedCell { get => _selectedCell; }
        #endregion

        #region events
        public event EventHandler<ChameleonBoard>? BoardChanged;
        public event EventHandler<Player>? CurrentPlayerChanged;
        public event EventHandler<Player>? GameOver;
        public event EventHandler<string>? ErrorOccurred;
        #endregion

        public GameModel(IChameleonDataAccess? dataAccess = null)
        {
            _dataAccess = dataAccess;
        }

        #region public methods
        public void NewGame(int size)
        {
            _board = new ChameleonBoard(size);
            _board.InitializePieces();
            _currentPlayer = Player.Red;

            isGameOver = false;
            Winner = null;

            OnBoardChange();
            OnCurrentPlayerChanged();
        }

        public void LoadGame(string path)
        {
            ChameleonBoardDTO loaded = _dataAccess!.LoadGame(path, out PlayerDTO currentPlayer);

            List<Piece> loadedPieces = new();
            foreach (PieceDTO pieceDTO in loaded.Pieces)
            {
                Player owner = pieceDTO.Owner == PlayerDTO.Red ? Player.Red : Player.Green;
                Piece piece = new(owner, pieceDTO.Row, pieceDTO.Col, pieceDTO.ColorChangeDelay);
                loadedPieces.Add(piece);
            }

            _board = new ChameleonBoard(loaded.Size, loadedPieces);

            _currentPlayer = currentPlayer == PlayerDTO.Red ? Player.Red : Player.Green;

            isGameOver = false;
            Winner = null;

            OnBoardChange();
            OnCurrentPlayerChanged();
        }

        public void SaveGame(string path)
        {
            List<PieceDTO> piecesDTO = new();
            for (int r = 0; r < _board!.Size; r++)
            {
                for (int c = 0; c < _board.Size; c++)
                {
                    Cell cell = _board[r, c];
                    if (cell.Piece != null)
                    {
                        PlayerDTO ownerDTO = cell.Piece.Owner == Player.Red ? PlayerDTO.Red : PlayerDTO.Green;
                        PieceDTO pieceDTO = new()
                        {
                            Owner = ownerDTO,
                            Row = cell.Row,
                            Col = cell.Col,
                            ColorChangeDelay = cell.Piece.ColorChangeDelay
                        };
                        piecesDTO.Add(pieceDTO);
                    }
                }
            }

            ChameleonBoardDTO boardDTO = new()
            {
                Size = _board!.Size,
                Pieces = piecesDTO
            };

            PlayerDTO currentPlayerDTO = _currentPlayer == Player.Red ? PlayerDTO.Red : PlayerDTO.Green;

            _dataAccess!.SaveGame(path, boardDTO, currentPlayerDTO);
        }

        public void EndTurn()
        {
            _board!.PerformColorChange();
            _currentPlayer = _currentPlayer == Player.Red ? Player.Green : Player.Red;

            isGameOver = IsGameOver();

            if (!isGameOver)
            {
                OnCurrentPlayerChanged();
            }
        }

        public bool IsGameOver()
        {
            bool redHasPieces = _board!.PlayerHasPieces(Player.Red);
            bool greenHasPieces = _board!.PlayerHasPieces(Player.Green);

            if (redHasPieces && !greenHasPieces && !isGameOver)
            {
                Winner = Player.Red;
                isGameOver = true;
                OnGameOver(Player.Red);
                return true;
            }

            if (greenHasPieces && !redHasPieces && !isGameOver)
            {
                Winner = Player.Green;
                isGameOver = true;
                OnGameOver(Player.Green);
                return true;
            }

            return false;
        }

        public void CellClicked(int r, int c)
        {
            if (isGameOver)
                return;

            if (_selectedCell == null)
            {
                Cell cell = _board![r, c];
                if (cell.Piece == null)
                {
                    OnErrorOccurred("Please select a cell that contains your piece.");
                    return;
                }

                if (cell.Piece.Owner != _currentPlayer)
                {
                    OnErrorOccurred("This is not your piece. Please select yours!");
                    return;
                }

                _selectedCell = (r, c);
                OnBoardChange();
            }
            else if (_selectedCell.Value.r == r && _selectedCell.Value.c == c)
            {
                _selectedCell = null;
                OnBoardChange();
            }
            else
            {
                Cell from = _board![_selectedCell.Value.r, _selectedCell.Value.c];
                Cell to = _board![r, c];
                try
                {
                    bool moved = _board!.TryMovePiece(from, to, _currentPlayer);
                    if (!moved)
                    {
                        bool jumped = _board!.TryJump(from, to, _currentPlayer);
                        if (!jumped)
                        {
                            OnErrorOccurred("Invalid move or jump!");
                            return;
                        }
                    }
                    _selectedCell = null;
                    EndTurn();
                    OnBoardChange();
                }
                catch (Exception ex)
                {
                    OnErrorOccurred(ex.Message);
                    _selectedCell = null;
                    return;
                }
            }
        }
        #endregion

        #region event invokers
        private void OnBoardChange()
        {
            BoardChanged?.Invoke(this, _board!);
        }

        private void OnCurrentPlayerChanged()
        {
            CurrentPlayerChanged?.Invoke(this, _currentPlayer);
        }

        private void OnGameOver(Player winner)
        {
            GameOver?.Invoke(this, winner);
        }

        private void OnErrorOccurred(string message)
        {
            ErrorOccurred?.Invoke(this, message);
        }
        #endregion
    }
}
