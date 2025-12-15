using ChameleonGame.Model;
using System;
using System.Collections.ObjectModel;

namespace ChameleonGame.ViewModels;

public class MainViewModel : ViewModelBase
{
    
    
    #region private fields

    private readonly GameModel _model;
    private bool _isGameOver = false;
    private string _currentPlayer = "";
    private int _boardSize;
    private NewGameWindowViewModel? _currentOverlay;

    #endregion

    #region public properties

    public ObservableCollection<BoardCellViewModel> BoardCells { get; private set; }
    public string CurrentPlayer
    {
        get => _currentPlayer;
        private set
        {
            _currentPlayer = value;
            OnPropertyChanged();
        }
    }
    public int BoardSize
    {
        get => _boardSize;
        private set
        {
            _boardSize = value;
            OnPropertyChanged();
        }
    }

    public bool IsNewGameOpen
    {
        get => CurrentOverlay != null;
    }

    public NewGameWindowViewModel? CurrentOverlay
    {
        get => _currentOverlay;
        set
        {
            _currentOverlay = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public DelegateCommand NewGameCommand { get; private set; }
    public DelegateCommand SaveGameCommand { get; private set; }
    public DelegateCommand LoadGameCommand { get; private set; }
    public DelegateCommand CellClickedCommand { get; private set; }

    #endregion

    #region Events

    //public event EventHandler? NewGame;
    public event EventHandler? SaveGame;
    public event EventHandler? LoadGame;
    public event EventHandler<string>? GameOver;
    public event EventHandler<string>? ErrorOccurred;

    #endregion

    public MainViewModel(GameModel model)
    {
        _model = model;
        BoardCells = new ObservableCollection<BoardCellViewModel>();

        _model.BoardChanged += RenderBoard;
        _model.CurrentPlayerChanged += CurrentPlayerChanged;
        _model.GameOver += OnGameOver;
        _model.ErrorOccurred += OnErrorOccurred;

        NewGameCommand = new DelegateCommand(param => {
            NewGameWindowViewModel vm = new NewGameWindowViewModel();

            vm.RequestStartGame += (s, e) =>
            {
                NewGameRequested(e);
                CurrentOverlay = null;
                OnPropertyChanged(nameof(IsNewGameOpen));
            };

            vm.RequestCancel += (s, e) =>
            {
                CurrentOverlay = null;
                OnPropertyChanged(nameof(IsNewGameOpen));
            };

            CurrentOverlay = vm;
            OnPropertyChanged(nameof(IsNewGameOpen));
        });
        SaveGameCommand = new DelegateCommand(param => {

            if (BoardCells.Count != 0)
            {
                string? path = param as string;
                if (string.IsNullOrEmpty(path))
                {
                    SaveGame?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    try
                    {
                        _model.SaveGame(path);
                    }
                    catch (Exception ex)
                    {
                        OnErrorOccurred(this, ex.Message);
                    }
                }
            }

        }, _ => !_isGameOver);
        LoadGameCommand = new DelegateCommand(param => {
            string? path = param as string;
            if (string.IsNullOrEmpty(path))
            {
                LoadGame?.Invoke(this, EventArgs.Empty);
                return;
            }
            else
            {
                try
                {
                    _model.LoadGame(path);
                    _isGameOver = false;
                    RefreshCommands();
                    return;
                }
                catch (Exception ex)
                {
                    OnErrorOccurred(this, ex.Message);
                }
            }
        });
        CellClickedCommand = new DelegateCommand(param => {
            if (param is BoardCellViewModel cell)
                _model.CellClicked(cell.Row, cell.Col);
        }, _ => !_isGameOver);

    }

    #region Private methods

    private void NewGameRequested(int size)
    {
        try
        {
            BoardSize = size!;
            GenerateBoard();
            _model.NewGame(size);
            _isGameOver = false;
            RefreshCommands();
        }
        catch (Exception ex)
        {
            OnErrorOccurred(this, ex.Message);
        }
    }

    private void GenerateBoard()
    {
        BoardCells.Clear();
        for (int r = 0; r < BoardSize; r++)
        {
            for (int c = 0; c < BoardSize; c++)
            {
                BoardCells.Add(new BoardCellViewModel(r, c, CellClickedCommand));
            }
        }
    }

    private void RefreshCommands()
    {
        SaveGameCommand.RaiseCanExecuteChanged();
        CellClickedCommand.RaiseCanExecuteChanged();
    }

    #endregion

    #region Event handlers

    private void RenderBoard(object? sender, ChameleonBoard board)
    {
        if (_model == null)
            return;

        if (BoardSize != board.Size)
        {
            BoardSize = board.Size;
            GenerateBoard();
        }

        foreach (var cellVM in BoardCells)
        {
            Cell cell = board.Board[cellVM.Row, cellVM.Col];
            cellVM.CellImageFilename = cell.Color.ToString();
            cellVM.PieceImageFilename = cell.Piece?.Owner.ToString() ?? null;

            if (_model.SelectedCell.HasValue)
            {
                cellVM.IsSelected = _model.SelectedCell.Value.r == cellVM.Row && _model.SelectedCell.Value.c == cellVM.Col;
            }
            else
            {
                cellVM.IsSelected = false;
            }
        }
    }

    private void CurrentPlayerChanged(object? sender, Player player)
    {
        CurrentPlayer = $"Turn: {(player == Player.Red ? "Red" : "Green")}";
    }

    private void OnGameOver(object? sender, Player e)
    {
        _isGameOver = true;
        RefreshCommands();

        CurrentPlayer = "Game is over!";

        string winnerString = e == Player.Red ? "Red" : "Green";
        GameOver?.Invoke(this, winnerString);
    }

    private void OnErrorOccurred(object? sender, string e)
    {
        ErrorOccurred?.Invoke(this, e);
    }

    #endregion
}
