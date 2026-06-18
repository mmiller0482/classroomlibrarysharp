using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ClassroomLibrary.Services;
using ClassroomLibrary.ViewModels;
using ClassroomLibrary.Views;

namespace ClassroomLibrary;

public partial class App : Application
{
    private SettingsWindow? _settingsWindow;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var service = new LibraryService();
            var themeService = new ThemeService();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(service, themeService)
            };
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void Settings_OnClick(object? sender, EventArgs e)
    {
        if (_settingsWindow is not null)
        {
            _settingsWindow.Activate();
            return;
        }

        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: { DataContext: MainWindowViewModel viewModel } owner
            })
        {
            return;
        }

        _settingsWindow = new SettingsWindow
        {
            DataContext = viewModel.Settings
        };
        _settingsWindow.Closed += (_, _) => _settingsWindow = null;
        _settingsWindow.Show(owner);
    }
}
