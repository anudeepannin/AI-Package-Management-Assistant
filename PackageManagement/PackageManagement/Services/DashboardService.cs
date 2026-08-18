using Microsoft.EntityFrameworkCore;
using PackageManagement.Data;

namespace PackageManagement.Services;

public class DashboardService
{
    private readonly PackageDbContext _context;

    public DashboardService(PackageDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetDashboardAsync()
    {
        var activePackages =
            await _context.Packages
                .CountAsync(x => x.Status == "Active");

        var expiredPackages =
            await _context.Packages
                .CountAsync(x => x.Status == "Expired");

        var openTickets =
            await _context.SupportTickets
                .CountAsync(x => x.Status == "Open");

        var pendingRenewals =
            await _context.RenewalRequests
                .CountAsync(x => x.Status == "Pending Approval");

        return new
        {
            activePackages,
            expiredPackages,
            openTickets,
            pendingRenewals
        };
    }
}