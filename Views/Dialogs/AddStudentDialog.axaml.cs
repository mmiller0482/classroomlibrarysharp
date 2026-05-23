using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.Models;

namespace ClassroomLibrary.Views.Dialogs;

public partial class AddStudentDialog : Window
{
    public AddStudentDialog() => InitializeComponent();

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        var name = NameBox.Text?.Trim();
        if (string.IsNullOrEmpty(name)) return;   // Name is required

        Close(new Student
        {
            Name  = name,
            Grade = GradeBox.Text?.Trim() ?? string.Empty,
            HomeroomTeacher = TeacherBox.Text?.Trim() ?? string.Empty
            
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);
}
