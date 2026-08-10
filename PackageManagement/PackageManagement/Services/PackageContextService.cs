using PackageManagement.Plugins;
using System.Text.RegularExpressions;

namespace PackageManagement.Services
{
    public class PackageContextService
    {
        private readonly PackagePlugin _packagePlugin;
        private readonly UserSessionService _session;

        public PackageContextService(PackagePlugin packagePlugin, UserSessionService session)
        {
            _packagePlugin = packagePlugin;
            _session = session;
        }

        //public string GetPackageInfo(string message)
        //{
        //    if (!message.Contains("package"))
        //        return "";

        //    return _packagePlugin.GetPackageStatus(1001);
        //}

        public string GetPackageInfo(string message)
        {
            if (!message.Contains("package"))
                return "";

            var match =
                Regex.Match(message, @"\d+");

            if (!match.Success)
                return "Please provide a package id.";

            int packageId =
                int.Parse(match.Value);
            _session.CurrentPackageId = packageId;

            return _packagePlugin.GetPackageStatus(
                packageId);
        }
        public string ActivatePackage(int packageId)
        {
            return _packagePlugin.ActivatePackage(packageId);
        }
    }
}
