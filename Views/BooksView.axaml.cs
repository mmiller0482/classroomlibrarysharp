using System.Linq;
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

    private async void OnRemoveBookClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not BooksViewModel vm) return;

        var ids = BooksGrid.SelectedItems
            .OfType<BookRow>()
            .Select(row => row.Id)
            .ToList();
        var removed = vm.RemoveBooks(ids);

        if (removed < ids.Count && GetWindow() is Window owner)
        {
            await MessageDialog.ShowAsync(
                owner,
                "Books Not Removed",
                "Checked-out books cannot be removed. Return them first and try again.");
        }
    }

    private Window? GetWindow() => TopLevel.GetTopLevel(this) as Window;
}
