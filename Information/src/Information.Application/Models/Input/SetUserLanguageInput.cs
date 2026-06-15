using Information.Application.Enums;

namespace Information.Application.Models.Input;

public record SetUserLanguageInput(long TelegramUserId, BotLanguage Language);
