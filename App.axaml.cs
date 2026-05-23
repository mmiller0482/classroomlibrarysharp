using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ClassroomLibrary.Services;
using ClassroomLibrary.ViewModels;
using ClassroomLibrary.Views;

namespace ClassroomLibrary;

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var service = new LibraryService();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(service)
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}
