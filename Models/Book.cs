using System;
using System.Text.Json.Serialization;

namespace ClassroomLibrary.Models;

public class Book
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string AuthorFirstName { get; set; } = string.Empty;
    public string AuthorLastName { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;

    [JsonIgnore]
    public string AuthorFullName => string.IsNullOrWhiteSpace(AuthorLastName)
        ? AuthorFirstName
        : $"{AuthorFirstName} {AuthorLastName}";

    public Book CreateCopy() => new()
    {
        Title = Title,
        AuthorFirstName = AuthorFirstName,
        AuthorLastName = AuthorLastName,
        ISBN = ISBN,
        Genre = Genre
    };

    public override string ToString() => string.IsNullOrEmpty(AuthorFullName)
        ? Title
        : $"{Title} — {AuthorFullName}";
}
