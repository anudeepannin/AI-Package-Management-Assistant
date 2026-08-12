using Microsoft.AspNetCore.Mvc;
using PackageManagement.Services;

namespace PackageManagement.Controllers;

[ApiController]
[Route("api/support")]
public class SupportController : ControllerBase
{
    private readonly SupportTicketService _service;

    public SupportController(
        SupportTicketService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(
        int packageId,
        string issue,
        string severity)
    {
        var ticketId =
            await _service.CreateTicketAsync(
                packageId,
                issue,
                severity);

        return Ok(ticketId);
    }

    [HttpPost("close")]
    public async Task<IActionResult> Close(
        string ticketId)
    {
        await _service.CloseTicketAsync(ticketId);

        return Ok("Closed");
    }
}
