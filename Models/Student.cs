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

    [JsonIgnore]
    public string FullName => string.IsNullOrWhiteSpace(LastName)
        ? FirstName
        : $"{FirstName} {LastName}";

    public override string ToString() => $"{FullName} ({Grade})";
}
