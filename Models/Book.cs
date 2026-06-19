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
    public int TotalCopies { get; set; } = 1;

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

    public override string ToString() => string.IsNullOrEmpty(AuthorFullName)
        ? Title
        : $"{Title} — {AuthorFullName}";
}
