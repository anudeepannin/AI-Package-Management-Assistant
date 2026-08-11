using PackageManagement.Services;

namespace PackageManagement.Agents;

public class OwnerAgent
{
    private readonly PackageContextService _packageService;

    public OwnerAgent(
        PackageContextService packageService)
    {
        _packageService = packageService;
    }

    public string Execute(int packageId)
    {
        return _packageService
            .GetPackageOwnerdetails(packageId);
    }
}