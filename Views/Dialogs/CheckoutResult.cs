using System;

namespace ClassroomLibrary.Views.Dialogs;

/// <summary>Data returned from <see cref="CheckoutDialog"/> on success.</summary>
public class CheckoutResult
{
    public string BookId    { get; init; } = string.Empty;
    public string StudentId { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
}
