using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageManagement.Data;

namespace PackageManagement.Controllers;

[ApiController]
[Route("api/package")]
public class PackageController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPackages(
        [FromServices] PackageDbContext db)
    {
        var packages =
            await db.Packages.ToListAsync();

        return Ok(packages);
    }
}