using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using ChameleonGame.Model;
using ChameleonGame.Persistance;
using ChameleonGame.ViewModels;
using ChameleonGame.Views;
using MsBox.Avalonia;
using System;

namespace ChameleonGame;

public partial class App : Application
{
    private IChameleonDataAccess _dataAccess = null!;
    private GameModel _model = null!;
    private MainViewModel _mainViewModel = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _dataAccess = new ChameleonTxtDataAccess();
            _model = new GameModel(_dataAccess);
            _mainViewModel = new MainViewModel(_model);

            //_mainViewModel.NewGame += ViewModel_NewGame;
            _mainViewModel.SaveGame += ViewModel_SaveGame; // Async void!
            _mainViewModel.LoadGame += ViewModel_LoadGame; // Async void!
            _mainViewModel.GameOver += ViewModel_GameOver;
            _mainViewModel.ErrorOccurred += ViewModel_ErrorOccurred;

            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    //private void ViewModel_NewGame(object? sender, EventArgs e)
    //{        
    //    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    //    {
    //        var view = new NewGameView();
    //        var viewModel = new NewGameWindowViewModel();
    //        view.DataContext = viewModel;

    //        var originalContent = desktop.MainWindow!.Content;

    //        viewModel.RequestStartGame += (s, e) =>
    //        {
    //            _mainViewModel.NewGameCommand.Execute(e);
    //            desktop.MainWindow.Content = originalContent;
    //        };

    //        viewModel.RequestCancel += (s, e) =>
    //        {
    //            desktop.MainWindow.Content = originalContent;
    //        };

    //        desktop.MainWindow.Content = view;
    //    }
    //}

    private async void ViewModel_SaveGame(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(desktop.MainWindow);
            if (topLevel == null) return;

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Game",
                FileTypeChoices = new[] { new FilePickerFileType("Chameleon Game") { Patterns = new[] { "*.txt" } } },
                DefaultExtension = "txt"
            });

            if (file != null)
            {
                try
                {
                    _mainViewModel.SaveGameCommand.Execute(file.Path.LocalPath);
                }
                catch (Exception ex)
                {
                    ViewModel_ErrorOccurred(this, ex.Message);
                }
            }
        }
    }

    private async void ViewModel_LoadGame(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // StorageProvider használata OpenFileDialog helyett
            var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(desktop.MainWindow);
            if (topLevel == null) return;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Load Game",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("Chameleon Game") { Patterns = new[] { "*.txt" } } }
            });

            if (files.Count >= 1)
            {
                try
                {
                    _mainViewModel.LoadGameCommand.Execute(files[0].Path.LocalPath);
                }
                catch (Exception ex)
                {
                    ViewModel_ErrorOccurred(this, ex.Message);
                }
            }
        }
    }

    private void ViewModel_GameOver(object? sender, string e)
    {
        MessageBoxManager.GetMessageBoxStandard("Game Over", $"{e} player wins!").ShowAsync();
    }

    private void ViewModel_ErrorOccurred(object? sender, string e)
    {
        MessageBoxManager.GetMessageBoxStandard("Error", e).ShowAsync();
    }
}
