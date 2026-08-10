using Microsoft.SemanticKernel;
using PackageManagement.Data;

namespace PackageManagement.Plugins
{
    public class PackagePlugin
    {

        private readonly PackageDbContext _dbContext;

        public PackagePlugin(PackageDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [KernelFunction]
        public string GetPackageStatus(int packageId)
        {
            var package = _dbContext.Packages
                .FirstOrDefault(x => x.PackageId == packageId);

            return package?.Status ?? "Package not found";
        }
        [KernelFunction]
        public string GetPackageOwner(int packageId)
        {
            var package = _dbContext.Packages
                .FirstOrDefault(x => x.PackageId == packageId);

            return package?.OwnerName ?? "Package not found";
        }

        public string ActivatePackage(int packageId)
        {
            var package = _dbContext.Packages
                .FirstOrDefault(x => x.PackageId == packageId);

            if (package == null)
                return "Package not found.";

            package.Status = "Active";

            _dbContext.SaveChanges();

            return "Package activated successfully.";
        }

        //[KernelFunction]
        //public string GetPackageStatus(string packageId)
        //{
        //    return $"Package {packageId} is Active";
        //}

        //[KernelFunction]
        //public string GetPackageOwner(string packageId)
        //{
        //    return $"Owner of package {packageId} is System";
        //}
    }
}
