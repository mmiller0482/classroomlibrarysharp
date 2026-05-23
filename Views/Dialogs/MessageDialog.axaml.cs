using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ClassroomLibrary.Views.Dialogs;

public partial class MessageDialog : Window
{
    public MessageDialog() => InitializeComponent();

    private void SetContent(string title, string body)
    {
        Title          = title;
        TitleText.Text = title;
        BodyText.Text  = body;
    }

    private void OnOkClick(object? sender, RoutedEventArgs e) => Close();

    /// <summary>Convenience helper — awaitable one-liner.</summary>
    public static Task ShowAsync(Window owner, string title, string body)
    {
        var dlg = new MessageDialog();
        dlg.SetContent(title, body);
        return dlg.ShowDialog(owner);
    }
}
