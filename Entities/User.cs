using System.ComponentModel.DataAnnotations;

namespace UnitTestExample.Entities;

public class TelegramUser
{
    public long Id { get; set; } 
    public long ChatId { get; set; }
    [MaxLength(255)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(255)]

    public string? LastName { get; set; }
    [MaxLength(255)]

    public string? Username { get; set; }
    [MaxLength(255)]

    public string? PhoneNumber { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
