using Microsoft.AspNetCore.Mvc;
using PackageManagement.Services;

namespace PackageManagement.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(
        DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard =
            await _dashboardService.GetDashboardAsync();

        return Ok(dashboard);
    }
}