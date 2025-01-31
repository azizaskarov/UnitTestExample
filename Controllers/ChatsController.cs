using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UnitTestExample.Hubs;

namespace UnitTestExample.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly IHubContext<QuestionHub> _hubContext;

    public ChatsController(IHubContext<QuestionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("SendQuestion")]
    public IActionResult SendQuestion([FromBody] QuestionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest("Savol bo'sh bo'lmasligi kerak.");
        }

        BackgroundJob.Schedule(() => SendQuestionToClient(request.ConnectionId, request.Question), TimeSpan.FromSeconds(5));

        return Ok(new { Message = "Savol qabul qilindi va javob yuboriladi." });
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public void SendQuestionToClient(string connectionId, string question)
    {
        _hubContext.Clients.Client(connectionId).SendAsync("Answer", question);
    }

    [HttpGet]
    public List<int> GetValues()
    {
        return Data.Values;
    }

}


public class QuestionRequest
{
    public string ConnectionId { get; set; }
    public string Question { get; set; }
}
public static class Data
{
    public static List<int> Values = [1, 2, 3];
}

