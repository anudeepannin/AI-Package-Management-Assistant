using Microsoft.AspNetCore.Mvc;
using PackageManagement.Models;
using PackageManagement.Services;

namespace PackageManagement.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Chat(
        ChatRequest request,
        [FromServices] ChatService chatService)
    {
        var response =
            await chatService
                .ChatWithSqlAsync(
                    request.Message);

        return Ok(response);
    }
}