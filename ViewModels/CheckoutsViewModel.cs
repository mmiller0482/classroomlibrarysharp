using System.Collections.ObjectModel;
using System.Linq;
using ClassroomLibrary.Models;
using ClassroomLibrary.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassroomLibrary.ViewModels;

public partial class CheckoutsViewModel : ObservableObject
{
    // Expose service so the View code-behind can pass books/students to the dialog.
    public LibraryService Service => _service;

    private readonly LibraryService _service;

    [ObservableProperty] private ObservableCollection<CheckoutRow> _activeCheckouts = [];
    [ObservableProperty] private CheckoutRow? _selectedCheckout;

    public CheckoutsViewModel(LibraryService service)
    {
        _service = service;
        Refresh();
    }

    public void Refresh()
    {
        ActiveCheckouts.Clear();

        var books    = _service.GetBooks().ToDictionary(b => b.Id);
        var students = _service.GetStudents().ToDictionary(s => s.Id);

        foreach (var checkout in _service.GetActiveCheckouts())
        {
            var bookTitle   = books.TryGetValue(checkout.BookId, out var b) ? b.Title : "Unknown";
            var studentName = students.TryGetValue(checkout.StudentId, out var s) ? s.Name : "Unknown";
            ActiveCheckouts.Add(new CheckoutRow(checkout, bookTitle, studentName));
        }
    }

    public void ReturnSelected()
    {
        if (SelectedCheckout is null) return;
        _service.ReturnBook(SelectedCheckout.Id);
        Refresh();
    }
}

/// <summary>Read-only display row for an active checkout.</summary>
public class CheckoutRow(Checkout checkout, string bookTitle, string studentName)
{
    public string Id          => checkout.Id;
    public string BookTitle   => bookTitle;
    public string StudentName => studentName;
    public string CheckoutDate => checkout.CheckoutDate.ToString("MMM d, yyyy");
    public string DueDate      => checkout.DueDate.ToString("MMM d, yyyy");
    public bool   IsOverdue    => checkout.IsOverdue;
    public string Status       => checkout.IsOverdue ? "OVERDUE" : "On Time";
}
