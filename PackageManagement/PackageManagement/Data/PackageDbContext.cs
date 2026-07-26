using Microsoft.EntityFrameworkCore;
using PackageManagement.Models;
namespace PackageManagement.Data
{
    public class PackageDbContext : DbContext
    {
        public PackageDbContext(DbContextOptions<PackageDbContext> options) : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }
    }
}
