using PackageManagement.Services;
namespace PackageManagement.Agents
{
    public class PackageAgent
    {
        private readonly PackageContextService _packageService;

        public PackageAgent(
            PackageContextService packageService)
        {
            _packageService = packageService;
        }

        public string Execute(string question)
        {
            return _packageService.GetPackageInfo(
                question);
        }
    }
}
