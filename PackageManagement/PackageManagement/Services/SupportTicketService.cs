using PackageManagement.Data;
using PackageManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace PackageManagement.Services;

public class SupportTicketService
{
    private readonly PackageDbContext _context;

    public SupportTicketService(
        PackageDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateTicketAsync(
        int packageId,
        string issue,
        string severity)
    {
        string ticketId =
            $"SUP-{DateTime.UtcNow:yyyyMMddHHmmss}";

        var ticket = new SupportTicket
        {
            TicketId = ticketId,
            PackageId = packageId,
            Issue = issue,
            Severity = severity,
            Status = "Open",
            CreatedDate = DateTime.UtcNow
        };

        _context.SupportTickets.Add(ticket);

        await _context.SaveChangesAsync();

        return ticketId;
    }

    public async Task<SupportTicket?> GetTicketAsync(
        string ticketId)
    {
        return await _context.SupportTickets.FirstOrDefaultAsync( x => x.TicketId == ticketId);
    }

    public async Task CloseTicketAsync(
        string ticketId)
    {
        var ticket =
            await GetTicketAsync(ticketId);

        if (ticket == null)
            return;

        ticket.Status = "Closed";

        await _context.SaveChangesAsync();
    }
    public async Task<List<SupportTicket>> GetAllTicketsAsync()
    {
        return await _context.SupportTickets
            .OrderByDescending(x => x.CreatedDate)
            .ToListAsync();
    }
}
