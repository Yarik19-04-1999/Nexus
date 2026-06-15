using Information.Application.Enums;
using Nexus.Application.Core.Interfaces;

namespace Information.Application.Models;

public class TelegramUser : IHasCreatedAt, IHasUpdatedAt
{
    public long TelegramUserId { get; set; }
    public BotLanguage Language { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
