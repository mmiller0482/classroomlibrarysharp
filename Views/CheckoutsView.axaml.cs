using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.ViewModels;
using ClassroomLibrary.Views.Dialogs;

namespace ClassroomLibrary.Views;

public partial class CheckoutsView : UserControl
{
    public CheckoutsView() => InitializeComponent();

    private async void OnCheckoutClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not CheckoutsViewModel vm) return;
        if (GetWindow() is not Window owner) return;

        var availableBooks = vm.Service.GetBooks()
            .Where(b => vm.Service.IsBookAvailable(b.Id))
            .ToList();
        var students = vm.Service.GetStudents().ToList();

        if (availableBooks.Count == 0 || students.Count == 0)
        {
            var msg = availableBooks.Count == 0
                ? "No books are currently available."
                : "No students registered yet.";
            await MessageDialog.ShowAsync(owner, "Cannot Checkout", msg);
            return;
        }

        var dialog = new CheckoutDialog(availableBooks, students);
        var result = await dialog.ShowDialog<CheckoutResult?>(owner);
        if (result is null) return;

        vm.Service.CheckoutBook(result.BookId, result.StudentId, result.DueDate);
        vm.Refresh();
    }

    private void OnReturnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CheckoutsViewModel vm) vm.ReturnSelected();
    }

    private Window? GetWindow() => TopLevel.GetTopLevel(this) as Window;
}
