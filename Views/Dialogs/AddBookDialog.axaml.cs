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
        var authorFirstName = StringUtils.CleanInputString(AuthorFirstNameBox.Text);
        var authorLastName = StringUtils.CleanInputString(AuthorLastNameBox.Text);
        var genre = StringUtils.CleanInputString(GenreBox.Text);
        var isbn = StringUtils.CleanInputString(ISBNBox.Text);

        if (!ValidateInputs(title, authorFirstName, authorLastName))
            return;

        Close(new Book
        {
            Title           = title,
            AuthorFirstName = authorFirstName,
            AuthorLastName  = authorLastName,
            Genre           = genre,
            ISBN            = isbn,
            TotalCopies     = (int)(CopiesBox.Value ?? 1m)
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);

    private bool ValidateInputs(string title, string authorFirstName, string authorLastName)
    {
        List<string> errors = [];
        Control? firstInvalidControl = null;

        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add("Title is required.");
            firstInvalidControl = TitleBox;
        }
        if (string.IsNullOrWhiteSpace(authorFirstName))
        {
            errors.Add("Author first name is required.");
            firstInvalidControl ??= AuthorFirstNameBox;
        }
        if (string.IsNullOrWhiteSpace(authorLastName))
        {
            errors.Add("Author last name is required.");
            firstInvalidControl ??= AuthorLastNameBox;
        }

        ErrorBox.Text = string.Join("\n", errors);
        ErrorBox.IsVisible = errors.Count > 0;
        firstInvalidControl?.Focus();

        return errors.Count == 0;
    }
}
