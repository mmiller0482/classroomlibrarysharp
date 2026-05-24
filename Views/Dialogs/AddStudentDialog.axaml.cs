using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.Models;

namespace ClassroomLibrary.Views.Dialogs;

public partial class AddStudentDialog : Window
{
    public AddStudentDialog() => InitializeComponent();

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        string name = CleanInputString(NameBox.Text);
        string homeroomTeacher = CleanInputString(TeacherBox.Text);
        string errorText = GenerateErrorText(name, homeroomTeacher);
        if (!string.IsNullOrEmpty(errorText))
        {
           ErrorBox.IsVisible = true;
           ErrorBox.Text = errorText;
           return;
        }

        Close(new Student
        {
            Name  = name,
            Grade = CleanInputString(GradeBox.Text),
            HomeroomTeacher = homeroomTeacher
            
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);

    private static string GenerateErrorText(string name, string homeroomTeacher)
    {
        List<string> errors = [];
        if (string.IsNullOrEmpty(name))
        {
            errors.Add("Name is required");
        }
        if (string.IsNullOrEmpty(homeroomTeacher))
        {
            errors.Add("Homeroom teacher is required");
        }
        return string.Join(", ", errors);
    }

    // TODO: This is widely usable; move to some shared library
    private static string CleanInputString(string? aString) 
        => aString?.Trim() ?? string.Empty; 
}
