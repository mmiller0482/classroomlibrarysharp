using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.Models;

namespace ClassroomLibrary.Views.Dialogs;

public partial class CheckoutDialog : Window
{
    public CheckoutDialog(IReadOnlyList<Book> books, IReadOnlyList<Student> students)
    {
        InitializeComponent();

        BookCombo.ItemsSource    = books;
        StudentCombo.ItemsSource = students;

        // Books and Students use ToString() for display (Title — Author Full Name / Full Name (Grade)).
        if (books.Count > 0)    BookCombo.SelectedIndex    = 0;
        if (students.Count > 0) StudentCombo.SelectedIndex = 0;

        DueDatePicker.SelectedDate = DateTime.Today.AddDays(14);
    }

    private void OnCheckoutClick(object? sender, RoutedEventArgs e)
    {
        if (BookCombo.SelectedItem is not Book book)       return;
        if (StudentCombo.SelectedItem is not Student student) return;

        var due = DueDatePicker.SelectedDate ?? DateTime.Today.AddDays(14);

        Close(new CheckoutResult
        {
            BookId    = book.Id,
            StudentId = student.Id,
            DueDate   = due
        });
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);
}
