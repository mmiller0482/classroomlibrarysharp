using System.Collections.Generic;
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

        if (!ValidateInputs(title, author))
            return;

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

    private bool ValidateInputs(string title, string author)
    {
        List<string> errors = [];
        Control? firstInvalidControl = null;

        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add("Title is required.");
            firstInvalidControl = TitleBox;
        }
        if (string.IsNullOrWhiteSpace(author))
        {
            errors.Add("Author is required.");
            firstInvalidControl ??= AuthorBox;
        }

        ErrorBox.Text = string.Join("\n", errors);
        ErrorBox.IsVisible = errors.Count > 0;
        firstInvalidControl?.Focus();

        return errors.Count == 0;
    }
}
