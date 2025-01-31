using Microsoft.EntityFrameworkCore;
using UnitTestExample.Data;
using UnitTestExample.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace UnitTestExample.Services
{
    public class TelegramUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly ITelegramBotClient _botClient;

        public TelegramUserService(AppDbContext dbContext, ITelegramBotClient botClient)
        {
            _dbContext = dbContext;
            _botClient = botClient; 
        }

        public async Task AddTelegramUserAsync(TelegramUser user)
        {
            _dbContext.TelegramUsers.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(long chatId)
        {
            return await _dbContext.TelegramUsers.AnyAsync(u => u.ChatId == chatId);
        }

        public async Task<IEnumerable<TelegramUserDto>> GetAllTelegramUsersAsync()
        {
            return await _dbContext.TelegramUsers.Select(u => new TelegramUserDto()
            {
                ChatId = u.ChatId,
                Username = u.Username,
                FirstName = u.FirstName,
                JoinedAt = u.JoinedAt
            }).ToListAsync();

        }

        public async Task<TelegramUserDto?> GetAllTelegramUserAsync(long chatId)
        {
            return await _dbContext.TelegramUsers.Select(u => new TelegramUserDto()
            {
                ChatId = u.ChatId,
                Username = u.Username,
                FirstName = u.FirstName,
                JoinedAt = u.JoinedAt
            }).FirstOrDefaultAsync(u => u.ChatId == chatId);
        }

        public async Task SendMessageTelegramUserAsync(SendMessageToUser sendMessage)
        {
            var chatId = sendMessage.ChatId;
            var message = sendMessage.Message;
            var image = sendMessage.Image;

            if (!string.IsNullOrEmpty(message))
            {
                await _botClient.SendMessage(chatId, message);
            }
            if (image != null)
            {
                using var stream = image.OpenReadStream();
                await _botClient.SendPhoto(
                    chatId: chatId,
                    photo: InputFile.FromStream(stream, image.FileName),
                    caption: message
                );
            }
           
        }
    }

    public class SendMessageToUser
    {
        public long ChatId { get; set; }
        public string? Message { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class TelegramUserDto
    {
        public long ChatId { get; set; }
        public string? Username { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}