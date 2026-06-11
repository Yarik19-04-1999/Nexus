using Dvizh.Application.Enums;

namespace Dvizh.Application.Models.Input;

public record RespondToInviteInput(string Code, InviteAnswer Answer);
