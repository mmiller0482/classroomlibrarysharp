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
        _data = Load(out var wasReset);
        if (wasReset) Save();
    }

    // ── Queries ──────────────────────────────────────────────────────────────

    public IReadOnlyList<Book> GetBooks() => _data.Books.AsReadOnly();

    public IReadOnlyList<Student> GetStudents() => _data.Students.AsReadOnly();

    public IReadOnlyList<Checkout> GetActiveCheckouts() =>
        _data.Checkouts.Where(c => !c.IsReturned).ToList();

    public bool IsBookAvailable(string bookId) =>
        _data.Books.Any(b => b.Id == bookId) &&
        _data.Checkouts.All(c => c.BookId != bookId || c.IsReturned);

    public int GetActiveCheckoutCountForStudent(string studentId) =>
        _data.Checkouts.Count(c => c.StudentId == studentId && !c.IsReturned);

    // ── Books ─────────────────────────────────────────────────────────────────

    public void AddBooks(Book book, int copyCount)
    {
        var count = Math.Max(1, copyCount);
        _data.Books.Add(book);
        for (var i = 1; i < count; i++)
            _data.Books.Add(book.CreateCopy());
        Save();
    }

    public int RemoveBooks(IEnumerable<string> bookIds)
    {
        var removableIds = bookIds
            .Where(IsBookAvailable)
            .ToHashSet();
        if (removableIds.Count == 0) return 0;

        var removed = _data.Books.RemoveAll(b => removableIds.Contains(b.Id));
        Save();
        return removed;
    }

    // ── Students ──────────────────────────────────────────────────────────────

    public void AddStudent(Student student) { _data.Students.Add(student); Save(); }

    public int RemoveStudents(IEnumerable<string> studentIds)
    {
        var ids = studentIds.ToHashSet();
        var removed = _data.Students.RemoveAll(s => ids.Contains(s.Id));
        if (removed == 0) return 0;

        Save();
        return removed;
    }

    // ── Checkouts ─────────────────────────────────────────────────────────────

    /// <summary>Returns false if this physical book is already checked out.</summary>
    public bool CheckoutBook(string bookId, string studentId, DateTime dueDate)
    {
        if (!IsBookAvailable(bookId)) return false;

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

    private LibraryData Load(out bool wasReset)
    {
        wasReset = false;
        if (!File.Exists(_dataPath)) return CreateEmptyData();

        try
        {
            var json = File.ReadAllText(_dataPath);
            var data = JsonSerializer.Deserialize<LibraryData>(json);
            if (data?.SchemaVersion == LibraryData.CurrentSchemaVersion)
                return data;
        }
        catch (JsonException)
        {
            // Invalid and obsolete databases are intentionally reset while the app
            // is pre-production. There is no legacy migration path.
        }

        wasReset = true;
        return CreateEmptyData();
    }

    private static LibraryData CreateEmptyData() => new()
    {
        SchemaVersion = LibraryData.CurrentSchemaVersion
    };

    private void Save()
    {
        var json = JsonSerializer.Serialize(_data, JsonOptions);
        File.WriteAllText(_dataPath, json);
    }
}
