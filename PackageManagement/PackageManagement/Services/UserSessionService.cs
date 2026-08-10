namespace PackageManagement.Services;

public class UserSessionService
{
    public int? CurrentPackageId { get; set; }

    public string? CurrentMenu { get; set; }

    public string? ActiveAgent { get; set; }
    public string? RenewalStep { get; set; }

    public string? SelectedDuration { get; set; }
}
