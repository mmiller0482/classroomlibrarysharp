using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.Models;
using ClassroomLibrary.Utils;

namespace ClassroomLibrary.Views.Dialogs;

public partial class AddStudentDialog : Window
{
    public AddStudentDialog() => InitializeComponent();

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        var name = StringUtils.CleanInputString(NameBox.Text);
        var grade = StringUtils.CleanInputString(GradeBox.Text);
        var homeroomTeacher = StringUtils.CleanInputString(TeacherBox.Text);

        if (!ValidateInputs(name, homeroomTeacher))
            return;

        Close(new Student
        {
            Name  = name,
            Grade = grade,
            HomeroomTeacher = homeroomTeacher
            
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);

    private bool ValidateInputs(string name, string homeroomTeacher)
    {
        List<string> errors = [];
        Control? firstInvalidControl = null;

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("Name is required.");
            firstInvalidControl = NameBox;
        }
        if (string.IsNullOrWhiteSpace(homeroomTeacher))
        {
            errors.Add("Homeroom teacher is required.");
            firstInvalidControl ??= TeacherBox;
        }

        ErrorBox.Text = string.Join("\n", errors);
        ErrorBox.IsVisible = errors.Count > 0;
        firstInvalidControl?.Focus();

        return errors.Count == 0;
    }
}
