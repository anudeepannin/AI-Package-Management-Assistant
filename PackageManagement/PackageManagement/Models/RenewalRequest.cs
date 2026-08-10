namespace PackageManagement.Models;

public class RenewalRequest
{
    public int Id { get; set; }

    public string RequestId { get; set; } = string.Empty;

    public int PackageId { get; set; }

    public string Duration { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}
