using Microsoft.EntityFrameworkCore;
using PackageManagement.Data;
using PackageManagement.Models;

namespace PackageManagement.Services;

public class ComplianceService
{
    private readonly PackageDbContext _context;
    private readonly RenewalRequestService _renewalRequestService;
    private readonly SupportTicketService _supportTicketService;

    public ComplianceService(
        PackageDbContext context, RenewalRequestService renewalRequestService,
        SupportTicketService supportTicketService)
    {
        _context = context;
        _renewalRequestService = renewalRequestService;
        _supportTicketService = supportTicketService;

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
    public async Task<List<ComplianceReport>> GetComplianceReportAsync()
    {
        var packages =
            await _context.Packages.ToListAsync();

        var tickets =
            await _supportTicketService.GetAllTicketsAsync();

        var renewals =
            await _renewalRequestService.GetAllRequestsAsync();

        var report = packages.Select(package =>
        {
            int openTickets =
                tickets.Count(ticket =>
                    ticket.PackageId == package.PackageId &&
                    ticket.Status == "Open");

            int pendingRenewals =
                renewals.Count(renewal =>
                    renewal.PackageId == package.PackageId &&
                    renewal.Status == "Pending Approval");

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
        }).ToList();

        return report;
    }
}
