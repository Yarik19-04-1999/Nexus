using Dvizh.Application.Enums;

namespace Dvizh.Application.Models.Input;

public record UpdateInviteInput(int Id, string Message, string? Description, DateTime? ExpiresAt, InviteLanguage Language, InviteMascot Mascot);
