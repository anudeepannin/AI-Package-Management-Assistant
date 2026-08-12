using Microsoft.EntityFrameworkCore;
using PackageManagement.Data;
using PackageManagement.Models;

namespace PackageManagement.Services;

public class ComplianceService
{
    private readonly PackageDbContext _context;

    public ComplianceService(
        PackageDbContext context)
    {
        _context = context;
    }

    public async Task<ComplianceReport?>
        GenerateReportAsync(int packageId)
    {
        var package =
            await _context.Packages
                .FirstOrDefaultAsync(
                    x => x.PackageId == packageId);

        if (package == null)
            return null;

        int openTickets =
            await _context.SupportTickets
                .CountAsync(x =>
                    x.PackageId == packageId &&
                    x.Status == "Open");

        int pendingRenewals =
            await _context.RenewalRequests
                .CountAsync(x =>
                    x.PackageId == packageId &&
                    x.Status == "Pending Approval");

        string complianceStatus =
            package.Status == "Active"
            && openTickets == 0
            && pendingRenewals == 0
                ? "PASS"
                : "FAIL";

        return new ComplianceReport
        {
            PackageId = package.PackageId,
            PackageStatus = package.Status,
            Owner = package.OwnerName,
            OpenTickets = openTickets,
            PendingRenewals = pendingRenewals,
            ComplianceStatus = complianceStatus
        };
    }
}
