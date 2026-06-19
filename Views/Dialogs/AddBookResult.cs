using ClassroomLibrary.Models;

namespace ClassroomLibrary.Views.Dialogs;

/// <summary>Book metadata and the number of physical instances to add.</summary>
public class AddBookResult
{
    public Book Book { get; init; } = new();
    public int CopyCount { get; init; } = 1;
}
