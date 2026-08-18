using Microsoft.AspNetCore.Mvc;
using PackageManagement.Services;

namespace PackageManagement.Controllers;



[ApiController]
[Route("api/compliance")]
public class ComplianceController : ControllerBase
{
    private readonly ComplianceService _service;

    public ComplianceController(
        ComplianceService service)
    {
        _service = service;
    }

    [HttpGet("report")]
    public async Task<IActionResult>
        GetReport(int packageId)
    {
        var report =
            await _service.GenerateReportAsync(
                packageId);

        if (report == null)
            return NotFound();

        return Ok(report);
    }
    [HttpGet("reportAll")]
    public async Task<IActionResult> GetComplianceReport()
    {
        var report =
            await _service.GetComplianceReportAsync();

        return Ok(report);
    }
}