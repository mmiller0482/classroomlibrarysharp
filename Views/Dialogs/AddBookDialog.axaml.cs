using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.Models;
using ClassroomLibrary.Utils;

namespace ClassroomLibrary.Views.Dialogs;

public partial class AddBookDialog : Window
{
    public AddBookDialog() => InitializeComponent();

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        var title = StringUtils.CleanInputString(TitleBox.Text);
        var author = StringUtils.CleanInputString(AuthorBox.Text);
        var genre = StringUtils.CleanInputString(GenreBox.Text);
        var isbn = StringUtils.CleanInputString(ISBNBox.Text);

        if (string.IsNullOrEmpty(title)) return;   // Title is required

        Close(new Book
        {
            Title       = title,
            Author      = author,
            Genre       = genre,
            ISBN        = isbn,
            TotalCopies = (int)(CopiesBox.Value ?? 1m)
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);
}
