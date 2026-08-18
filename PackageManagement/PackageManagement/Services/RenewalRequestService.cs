using PackageManagement.Data;
using PackageManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace PackageManagement.Services
{
    public class RenewalRequestService
    {
        private readonly PackageDbContext _context;

        public RenewalRequestService(
            PackageDbContext context)
        {
            _context = context;
        }

        public async Task CreateRequestAsync(
            string requestId,
            int packageId,
            string duration)
        {
            var request = new RenewalRequest
            {
                RequestId = requestId,
                PackageId = packageId,
                Duration = duration,
                Status = "Pending Approval",
                CreatedDate = DateTime.UtcNow
            };

            _context.RenewalRequests.Add(request);

            await _context.SaveChangesAsync();
        }

        public async Task<RenewalRequest?> GetLatestRequestAsync(int packageId)
        {
            return await _context.RenewalRequests
                .Where(x => x.PackageId == packageId)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateStatusAsync( string requestId, string status)
        {
            var request =
                await _context.RenewalRequests
                    .FirstOrDefaultAsync(
                        x => x.RequestId == requestId);

            if (request == null)
                return false;

            request.Status = status;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<RenewalRequest?> GetRequestByIdAsync( string requestId)
        {
            return await _context.RenewalRequests
                .FirstOrDefaultAsync(x =>
                    x.RequestId == requestId);
        }
        public async Task<List<RenewalRequest>> GetAllRequestsAsync()
        {
            return await _context.RenewalRequests
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }

}