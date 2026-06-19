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
        if (MigrateBookInstances()) Save();
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

    public bool RemoveBook(string bookId)
    {
        if (!IsBookAvailable(bookId)) return false;

        _data.Books.RemoveAll(b => b.Id == bookId);
        Save();
        return true;
    }

    // ── Students ──────────────────────────────────────────────────────────────

    public void AddStudent(Student student) { _data.Students.Add(student); Save(); }

    public void RemoveStudent(string studentId)
    {
        _data.Students.RemoveAll(s => s.Id == studentId);
        Save();
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

    /// <summary>
    /// Expands legacy quantity-based books into physical instances and assigns each
    /// historical checkout to a concrete instance without overlapping loans.
    /// </summary>
    private bool MigrateBookInstances()
    {
        var migrated = false;

        foreach (var book in _data.Books.ToList())
        {
            var copyCount = book.ImportedCopyCount;
            if (copyCount <= 1) continue;

            migrated = true;
            var instances = new List<Book> { book };
            for (var i = 1; i < copyCount; i++)
            {
                var copy = book.CreateCopy();
                instances.Add(copy);
                _data.Books.Add(copy);
            }

            var checkouts = _data.Checkouts
                .Where(c => c.BookId == book.Id)
                .OrderBy(c => c.CheckoutDate)
                .ThenBy(c => c.Id)
                .ToList();
            var availableOn = instances.ToDictionary(b => b.Id, _ => DateTime.MinValue);

            foreach (var checkout in checkouts)
            {
                var instance = instances.FirstOrDefault(candidate =>
                    availableOn[candidate.Id] <= checkout.CheckoutDate);

                // Preserve inconsistent legacy data rather than assigning overlapping
                // checkouts to the same physical object.
                if (instance is null)
                {
                    instance = book.CreateCopy();
                    instances.Add(instance);
                    _data.Books.Add(instance);
                    availableOn[instance.Id] = DateTime.MinValue;
                }

                checkout.BookId = instance.Id;
                availableOn[instance.Id] = checkout.ReturnDate ?? DateTime.MaxValue;
            }
        }

        return migrated;
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_data, JsonOptions);
        File.WriteAllText(_dataPath, json);
    }
}
