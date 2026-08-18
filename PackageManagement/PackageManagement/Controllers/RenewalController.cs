using Microsoft.AspNetCore.Mvc;
using PackageManagement.Services;

namespace PackageManagement.Controllers;

[ApiController]
[Route("api/renewal")]
public class RenewalController : ControllerBase
{
    private readonly RenewalRequestService _renewalRequestService;
    private readonly PackageContextService _packageContextService;

    public RenewalController(
        RenewalRequestService renewalRequestService,
        PackageContextService packageContextService)
    {
        _renewalRequestService = renewalRequestService;
        _packageContextService = packageContextService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRenewal(int packageId,string duration)
    {
        var requestId = $"REN-{DateTime.UtcNow:yyyyMMddHHmmss}";

        await _renewalRequestService.CreateRequestAsync(requestId,packageId,duration);

        return Ok(new
        {
            RequestId = requestId,
            PackageId = packageId,
            Duration = duration,
            Status = "Pending Approval"
        });
    }

    [HttpPost("approve")]
    public async Task<IActionResult> Approve(
        string requestId)
    {
        var success =
            await _renewalRequestService.UpdateStatusAsync(requestId,"Approved");

        if (!success)
        {
            return NotFound("Request not found");
        }

        var request =
            await _renewalRequestService.GetRequestByIdAsync(requestId);

        if (request != null)
        {
            _packageContextService.ActivatePackage(request.PackageId);
        }

        return Ok(new
        {
            RequestId = requestId,
            Status = "Approved",
            PackageStatus = "Active"
        });
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _renewalRequestService.GetAllRequestsAsync();

        return Ok(requests);
    }
}
