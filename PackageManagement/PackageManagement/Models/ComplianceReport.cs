namespace PackageManagement.Models;

public class ComplianceReport
{
    public int PackageId { get; set; }

    public string PackageStatus { get; set; } = "";

    public string Owner { get; set; } = "";

    public int OpenTickets { get; set; }

    public int PendingRenewals { get; set; }

    public string ComplianceStatus { get; set; } = "";
}
