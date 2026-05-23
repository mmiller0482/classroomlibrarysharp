using System;

namespace ClassroomLibrary.Models;

public class Book
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int TotalCopies { get; set; } = 1;

    public override string ToString() => string.IsNullOrEmpty(Author)
        ? Title
        : $"{Title} — {Author}";
}
