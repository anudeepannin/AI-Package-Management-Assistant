namespace PackageManagement.Models;

public class SupportTicket
{
    public int Id { get; set; }

    public string TicketId { get; set; } = "";

    public int PackageId { get; set; }

    public string Issue { get; set; } = "";

    public string Severity { get; set; } = "";

    public string Status { get; set; } = "Open";

    public DateTime CreatedDate { get; set; }
}
