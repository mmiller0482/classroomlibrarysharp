using System;
using System.Text.Json.Serialization;

namespace ClassroomLibrary.Models;

public class Student
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string HomeroomTeacher { get; set; } = string.Empty;

    // Read the old single Name field as FirstName without attempting to split it.
    [JsonPropertyName("Name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LegacyName
    {
        get => null;
        set
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                FirstName = value ?? string.Empty;
            }
        }
    }

    [JsonIgnore]
    public string FullName => string.IsNullOrWhiteSpace(LastName)
        ? FirstName
        : $"{FirstName} {LastName}";

    public override string ToString() => $"{FullName} ({Grade})";
}
