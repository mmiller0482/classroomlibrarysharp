using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassroomLibrary.ViewModels;
using ClassroomLibrary.Views.Dialogs;

namespace ClassroomLibrary.Views;

public partial class StudentsView : UserControl
{
    public StudentsView() => InitializeComponent();

    private async void OnAddStudentClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not StudentsViewModel vm) return;
        if (GetWindow() is not Window owner) return;
        var dialog = new AddStudentDialog();
        var result = await dialog.ShowDialog<Models.Student?>(owner);
        if (result is not null) vm.AddStudent(result);
    }

    private void OnRemoveStudentClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is StudentsViewModel vm) vm.RemoveSelected();
    }

    private Window? GetWindow() => TopLevel.GetTopLevel(this) as Window;
}
