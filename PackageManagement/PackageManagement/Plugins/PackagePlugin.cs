using Microsoft.SemanticKernel;


namespace PackageManagement.Plugins
{
    public class PackagePlugin
    {
        [KernelFunction]
        public string GetPackageStatus(string packageId)
        {
            return $"Package {packageId} is Active";
        }

        [KernelFunction]
        public string GetPackageOwner(string packageId)
        {
            return $"Owner of package {packageId} is System";
        }
    }
}
