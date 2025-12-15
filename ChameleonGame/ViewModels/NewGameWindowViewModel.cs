using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChameleonGame.ViewModels
{
    public class NewGameWindowViewModel : ViewModelBase
    {
        private int? _selectedSize = null;

        public int? SelectedSize
        {
            get => _selectedSize;
            set
            {
                if (_selectedSize != value)
                {
                    _selectedSize = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsEasy));
                    OnPropertyChanged(nameof(IsMedium));
                    OnPropertyChanged(nameof(IsHard));
                    ConfirmCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsEasy
        {
            get => SelectedSize == 3;
            set { if (value) SelectedSize = 3; }
        }

        public bool IsMedium
        {
            get => SelectedSize == 5;
            set { if (value) SelectedSize = 5; }
        }

        public bool IsHard
        {
            get => SelectedSize == 7;
            set { if (value) SelectedSize = 7; }
        }

        public event EventHandler<int>? RequestStartGame;
        public event EventHandler? RequestCancel;

        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public NewGameWindowViewModel()
        {
            ConfirmCommand = new DelegateCommand(_ => RequestStartGame?.Invoke(this, SelectedSize!.Value), _ => SelectedSize == 3 || SelectedSize == 5 || SelectedSize == 7);
            CancelCommand = new DelegateCommand(_ => RequestCancel?.Invoke(this, EventArgs.Empty));
        }
    }
}
