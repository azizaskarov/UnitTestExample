using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UnitTestExample.Entities;
using UnitTestExample.Services;

namespace UnitTestExample.BackgroundServices
{
    public class TelegramBackgroundService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<TelegramBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private CancellationTokenSource? _cts;

        public TelegramBackgroundService(
            ITelegramBotClient botClient,
            ILogger<TelegramBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _botClient = botClient; // DI orqali injeksiya qilingan
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Telegram bot is starting...");
            _cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Telegram bot is stopping...");
            _cts?.Cancel();
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { } message)
            {
                long chatId = message.Chat.Id;
                string firstName = message.Chat.FirstName ?? "";
                string? username = message.Chat.Username;
                string? lastName = message.Chat.LastName;

                _logger.LogInformation($"New message from {chatId}: {message.Text}");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var telegramUserService = scope.ServiceProvider.GetRequiredService<TelegramUserService>();

                    if (!await telegramUserService.ExistAsync(chatId))
                    {
                        await telegramUserService.AddTelegramUserAsync(new TelegramUser
                        {
                            ChatId = chatId,
                            FirstName = firstName,
                            Username = username,
                            LastName = lastName,
                            IsAdmin = false,
                            JoinedAt = DateTime.UtcNow,
                        });

                        await botClient.SendMessage(chatId, "Sizning ma'lumotlaringiz saqlandi!", cancellationToken: cancellationToken);
                    }
                }

                if (!string.IsNullOrEmpty(update.Message.Text))
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        await botClient.SendMessage(1855076083, $"{username}: {update.Message.Text!}", cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await botClient.SendMessage(1855076083, $"{firstName}: {update.Message.Text!}", cancellationToken: cancellationToken);
                    }
                }

                //if (update.Message.Photo != null)
                //{
                //    if (!string.IsNullOrEmpty(username))
                //    {
                //        await botClient.SendPhoto(1855076083,update.Message.Photo,  cancellationToken: cancellationToken);
                //    }
                //    else
                //    {
                //        await botClient.SendMessage(1855076083, $"`{firstName}: {update.Message.Text!}", cancellationToken: cancellationToken);
                //    }
                //}

                //long adminChatId = 1855076083;
                //await botClient.ForwardMessage(
                //    chatId: adminChatId,  // Adminga forward qilamiz
                //    fromChatId: chatId,   // Asl xabar jo‘natuvchisi
                //    messageId: message.MessageId, // Xabar ID-si
                //    cancellationToken: cancellationToken
                //);
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Telegram botda xatolik yuz berdi.");
            return Task.CompletedTask;
        }
    }
}
