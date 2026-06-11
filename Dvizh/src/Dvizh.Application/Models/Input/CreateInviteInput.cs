namespace Dvizh.Application.Models.Input;

public record CreateInviteInput(string Message, string? Description, DateTime? ExpiresAt);
