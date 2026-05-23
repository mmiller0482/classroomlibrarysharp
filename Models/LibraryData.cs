using System.Collections.Generic;

namespace ClassroomLibrary.Models;

/// <summary>Root container serialized to library.json on disk.</summary>
public class LibraryData
{
    public List<Book> Books { get; set; } = [];
    public List<Student> Students { get; set; } = [];
    public List<Checkout> Checkouts { get; set; } = [];
}
