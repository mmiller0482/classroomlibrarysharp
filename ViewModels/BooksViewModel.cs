using System.Collections.ObjectModel;
using ClassroomLibrary.Models;
using ClassroomLibrary.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassroomLibrary.ViewModels;

public partial class BooksViewModel : ObservableObject
{
    private readonly LibraryService _service;

    [ObservableProperty] private ObservableCollection<BookRow> _books = [];
    [ObservableProperty] private BookRow? _selectedBook;

    public BooksViewModel(LibraryService service)
    {
        _service = service;
        Refresh();
    }

    public void Refresh()
    {
        Books.Clear();
        foreach (var book in _service.GetBooks())
            Books.Add(new BookRow(book, _service.IsBookAvailable(book.Id)));
    }

    public void AddBooks(Book book, int copyCount)
    {
        _service.AddBooks(book, copyCount);
        Refresh();
    }

    public void RemoveSelected()
    {
        if (SelectedBook is null) return;
        _service.RemoveBook(SelectedBook.Id);
        Refresh();
    }
}

/// <summary>Read-only display row combining Book data with live availability.</summary>
public class BookRow(Book book, bool isAvailable)
{
    public string Id           => book.Id;
    public string Title        => book.Title;
    public string AuthorFirstName => book.AuthorFirstName;
    public string AuthorLastName  => book.AuthorLastName;
    public string ISBN         => book.ISBN;
    public string Genre        => book.Genre;
    public string Status       => isAvailable ? "Available" : "Checked Out";
}
