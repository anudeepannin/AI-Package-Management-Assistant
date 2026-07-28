using Microsoft.AspNetCore.Mvc;
using PackageManagement.Models;
using PackageManagement.Services;

namespace PackageManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat(
        [FromBody] ChatRequest request)
    {
        var response =
            await _chatService.AskAsync(
                request.Message);

        return Ok(response);
    }
}