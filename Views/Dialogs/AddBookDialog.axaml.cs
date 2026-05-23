using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.Models;

namespace ClassroomLibrary.Views.Dialogs;

public partial class AddBookDialog : Window
{
    public AddBookDialog() => InitializeComponent();

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        var title = TitleBox.Text?.Trim();
        if (string.IsNullOrEmpty(title)) return;   // Title is required

        Close(new Book
        {
            Title       = title,
            Author      = AuthorBox.Text?.Trim() ?? string.Empty,
            Genre       = GenreBox.Text?.Trim()  ?? string.Empty,
            ISBN        = ISBNBox.Text?.Trim()   ?? string.Empty,
            TotalCopies = (int)(CopiesBox.Value ?? 1m)
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);
}
