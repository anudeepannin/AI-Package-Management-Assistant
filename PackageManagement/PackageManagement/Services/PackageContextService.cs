using PackageManagement.Plugins;

namespace PackageManagement.Services
{
    public class PackageContextService
    {
        private readonly PackagePlugin _packagePlugin;

        public PackageContextService(PackagePlugin packagePlugin)
        {
            _packagePlugin = packagePlugin;
        }

        public string GetPackageInfo(string message)
        {
            if (!message.Contains("package"))
                return "";

            return _packagePlugin.GetPackageStatus(1001);
        }
    }
}
