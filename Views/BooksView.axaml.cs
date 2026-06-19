using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.ViewModels;
using ClassroomLibrary.Views.Dialogs;

namespace ClassroomLibrary.Views;

public partial class BooksView : UserControl
{
    public BooksView() => InitializeComponent();

    private async void OnAddBookClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not BooksViewModel vm) return;
        if (GetWindow() is not Window owner) return;
        var dialog = new AddBookDialog();
        var result = await dialog.ShowDialog<AddBookResult?>(owner);
        if (result is not null) vm.AddBooks(result.Book, result.CopyCount);
    }

    private void OnRemoveBookClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is BooksViewModel vm) vm.RemoveSelected();
    }

    private Window? GetWindow() => TopLevel.GetTopLevel(this) as Window;
}
