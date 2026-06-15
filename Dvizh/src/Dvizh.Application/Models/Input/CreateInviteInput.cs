namespace Dvizh.Application.Models.Input;

using Dvizh.Application.Enums;

namespace Dvizh.Application.Models.Input;

public record CreateInviteInput(string Message, string? Description, DateTime? ExpiresAt, InviteLanguage Language);
