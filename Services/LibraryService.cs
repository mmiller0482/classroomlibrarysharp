using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ClassroomLibrary.Models;

namespace ClassroomLibrary.Services;

/// <summary>
/// Single source-of-truth for all library data.
/// Persists to ~/AppData/Roaming/ClassroomLibrary/library.json (cross-platform).
/// </summary>
public class LibraryService
{
    private readonly string _dataPath;
    private LibraryData _data;

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public LibraryService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "ClassroomLibrary");
        Directory.CreateDirectory(dir);
        _dataPath = Path.Combine(dir, "library.json");
        _data = Load();
    }

    // ── Queries ──────────────────────────────────────────────────────────────

    public IReadOnlyList<Book> GetBooks() => _data.Books.AsReadOnly();

    public IReadOnlyList<Student> GetStudents() => _data.Students.AsReadOnly();

    public IReadOnlyList<Checkout> GetActiveCheckouts() =>
        _data.Checkouts.Where(c => !c.IsReturned).ToList();

    public int GetAvailableCopies(string bookId)
    {
        var book = _data.Books.FirstOrDefault(b => b.Id == bookId);
        if (book is null) return 0;
        var checkedOut = _data.Checkouts.Count(c => c.BookId == bookId && !c.IsReturned);
        return Math.Max(0, book.TotalCopies - checkedOut);
    }

    public int GetActiveCheckoutCountForStudent(string studentId) =>
        _data.Checkouts.Count(c => c.StudentId == studentId && !c.IsReturned);

    // ── Books ─────────────────────────────────────────────────────────────────

    public void AddBook(Book book) { _data.Books.Add(book); Save(); }

    public void RemoveBook(string bookId)
    {
        _data.Books.RemoveAll(b => b.Id == bookId);
        Save();
    }

    // ── Students ──────────────────────────────────────────────────────────────

    public void AddStudent(Student student) { _data.Students.Add(student); Save(); }

    public void RemoveStudent(string studentId)
    {
        _data.Students.RemoveAll(s => s.Id == studentId);
        Save();
    }

    // ── Checkouts ─────────────────────────────────────────────────────────────

    /// <summary>Returns false if no copies are available.</summary>
    public bool CheckoutBook(string bookId, string studentId, DateTime dueDate)
    {
        if (GetAvailableCopies(bookId) <= 0) return false;

        _data.Checkouts.Add(new Checkout
        {
            BookId = bookId,
            StudentId = studentId,
            CheckoutDate = DateTime.Today,
            DueDate = dueDate
        });
        Save();
        return true;
    }

    public void ReturnBook(string checkoutId)
    {
        var checkout = _data.Checkouts.FirstOrDefault(c => c.Id == checkoutId);
        if (checkout is null) return;
        checkout.ReturnDate = DateTime.Today;
        Save();
    }

    // ── Persistence ───────────────────────────────────────────────────────────

    private LibraryData Load()
    {
        if (!File.Exists(_dataPath)) return new LibraryData();
        try
        {
            var json = File.ReadAllText(_dataPath);
            return JsonSerializer.Deserialize<LibraryData>(json) ?? new LibraryData();
        }
        catch
        {
            return new LibraryData();
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_data, JsonOptions);
        File.WriteAllText(_dataPath, json);
    }
}
