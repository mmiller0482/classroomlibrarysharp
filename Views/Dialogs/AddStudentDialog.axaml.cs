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
        var firstName = StringUtils.CleanInputString(FirstNameBox.Text);
        var lastName = StringUtils.CleanInputString(LastNameBox.Text);
        var grade = StringUtils.CleanInputString(GradeBox.Text);
        var homeroomTeacher = StringUtils.CleanInputString(TeacherBox.Text);

        if (!ValidateInputs(firstName, lastName, homeroomTeacher))
            return;

        Close(new Student
        {
            FirstName = firstName,
            LastName = lastName,
            Grade = grade,
            HomeroomTeacher = homeroomTeacher
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);

    private bool ValidateInputs(string firstName, string lastName, string homeroomTeacher)
    {
        List<string> errors = [];
        Control? firstInvalidControl = null;

        if (string.IsNullOrWhiteSpace(firstName))
        {
            errors.Add("First name is required.");
            firstInvalidControl = FirstNameBox;
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add("Last name is required.");
            firstInvalidControl ??= LastNameBox;
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
