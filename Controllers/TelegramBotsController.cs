using Microsoft.AspNetCore.Mvc;
using UnitTestExample.Services;

namespace UnitTestExample.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TelegramBotsController : ControllerBase
{
    private readonly TelegramUserService telegramUserService;
    private readonly ILogger<TelegramBotsController> _logger;

    public TelegramBotsController(
        TelegramUserService telegramUserService,
        ILogger<TelegramBotsController> logger,
        IServiceProvider serviceProvider)
    {
        this.telegramUserService = telegramUserService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageToUserAsync(SendMessageToUser sendMessage)
    {
        var chatId = sendMessage.ChatId;
        var message = sendMessage.Message;

        if (string.IsNullOrEmpty(message))
        {
            return BadRequest("Message is required");
        }

        await telegramUserService.SendMessageTelegramUserAsync(sendMessage);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTelegramUsersAsync()
    {
        var users = await telegramUserService.GetAllTelegramUsersAsync();
        return Ok(users);
    }

    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetTelegramUserAsync(long chatId)
    {
        var user = await telegramUserService.GetAllTelegramUserAsync(chatId);

        if (user == null)
            return NotFound("User NotFound!");
        return Ok(user);
    }

}
