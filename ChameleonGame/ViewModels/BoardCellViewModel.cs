using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameleonGame.ViewModels
{
    public class BoardCellViewModel : ViewModelBase
    {
        private bool _isSelected = false;
        private string _cellImageFilename = "cell_gray.png";
        private string? _pieceImageFilename = null;

        public int Row { get; }
        public int Col { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CellImageFilename
        {
            get => _cellImageFilename;
            set
            {
                if (_cellImageFilename != value)
                {
                    _cellImageFilename = TransformCellPath(value);
                    OnPropertyChanged();
                }
            }
        }

        public string? PieceImageFilename
        {
            get => _pieceImageFilename;
            set
            {
                if (_pieceImageFilename != value)
                {
                    _pieceImageFilename = TransformPiecePath(value);
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand CellClickCommand { get; set; }

        public BoardCellViewModel(int row, int col, DelegateCommand cellClickCommand)
        {
            Row = row;
            Col = col;
            CellClickCommand = cellClickCommand;
        }

        private string TransformCellPath(string color)
        {
            return $"cell_{color.ToLower()}.png";
        }

        private string? TransformPiecePath(string? color)
        {
            return color != null ? $"chameleon_{color.ToLower()}.png" : null;
        }
    }

}
