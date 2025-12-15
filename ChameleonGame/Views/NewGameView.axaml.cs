using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChameleonGame.Views;

public partial class NewGameView : UserControl
{
    public NewGameView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}