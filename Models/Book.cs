using System;
using System.Text.Json.Serialization;

namespace ClassroomLibrary.Models;

public class Book
{
    private int _legacyCopyCount = 1;

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string AuthorFirstName { get; set; } = string.Empty;
    public string AuthorLastName { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;

    // Import the old quantity-based format. New data stores one Book per physical copy.
    [JsonPropertyName("TotalCopies")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int LegacyCopyCount
    {
        get => 0;
        set => _legacyCopyCount = Math.Max(1, value);
    }

    [JsonIgnore]
    public int ImportedCopyCount => _legacyCopyCount;

    // Read the old single Author field as AuthorFirstName without attempting to split it.
    [JsonPropertyName("Author")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LegacyAuthor
    {
        get => null;
        set
        {
            if (string.IsNullOrWhiteSpace(AuthorFirstName))
            {
                AuthorFirstName = value ?? string.Empty;
            }
        }
    }

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
