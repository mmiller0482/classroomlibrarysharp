using System;

namespace ClassroomLibrary.Models;

public class Checkout
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string BookId { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public DateTime CheckoutDate { get; set; } = DateTime.Today;
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);
    public DateTime? ReturnDate { get; set; }

    public bool IsReturned => ReturnDate.HasValue;
    public bool IsOverdue => !IsReturned && DateTime.Today > DueDate;
}
