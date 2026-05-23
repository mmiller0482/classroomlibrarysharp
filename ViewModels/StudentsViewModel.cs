using System.Collections.ObjectModel;
using ClassroomLibrary.Models;
using ClassroomLibrary.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClassroomLibrary.ViewModels;

public partial class StudentsViewModel : ObservableObject
{
    private readonly LibraryService _service;

    [ObservableProperty] private ObservableCollection<StudentRow> _students = [];
    [ObservableProperty] private StudentRow? _selectedStudent;

    public StudentsViewModel(LibraryService service)
    {
        _service = service;
        Refresh();
    }

    public void Refresh()
    {
        Students.Clear();
        foreach (var student in _service.GetStudents())
        {
            var booksOut = _service.GetActiveCheckoutCountForStudent(student.Id);
            Students.Add(new StudentRow(student, booksOut));
        }
    }

    public void AddStudent(Student student)
    {
        _service.AddStudent(student);
        Refresh();
    }

    public void RemoveSelected()
    {
        if (SelectedStudent is null) return;
        _service.RemoveStudent(SelectedStudent.Id);
        Refresh();
    }
}

/// <summary>Read-only display row combining Student data with live checkout count.</summary>
public class StudentRow(Student student, int booksOut)
{
    public string Id       => student.Id;
    public string Name     => student.Name;
    public string Grade    => student.Grade;
    public int    BooksOut => booksOut;
}
