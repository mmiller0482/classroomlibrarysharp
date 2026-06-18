using ClassroomLibrary.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassroomLibrary.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public BooksViewModel    Books    { get; }
    public StudentsViewModel Students { get; }
    public CheckoutsViewModel Checkouts { get; }
    public SettingsViewModel Settings { get; }

    [ObservableProperty] private int _selectedTabIndex;

    public MainWindowViewModel(LibraryService service, ThemeService themeService)
    {
        Books     = new BooksViewModel(service);
        Students  = new StudentsViewModel(service);
        Checkouts = new CheckoutsViewModel(service);
        Settings  = new SettingsViewModel(themeService);
    }

    // Refresh the newly-active tab so it always shows current data.
    partial void OnSelectedTabIndexChanged(int value)
    {
        switch (value)
        {
            case 0: Books.Refresh();     break;
            case 1: Students.Refresh();  break;
            case 2: Checkouts.Refresh(); break;
        }
    }
}
