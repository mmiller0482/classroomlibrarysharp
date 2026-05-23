using System;

namespace ClassroomLibrary.Models;

public class Student
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string HomeroomTeacher { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Name: {Name}, Grade: {Grade}, Homeroom Teacher: {HomeroomTeacher}";
    } 
}
