using Dvizh.Application.Enums;
using Nexus.Application.Core.Interfaces;

namespace Dvizh.Application.Models;

public class Invite : IHasCreatedAt, IHasUpdatedAt, IHasPrimaryIdentifier, IHasExpiresAt
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public InviteAnswer Answer { get; set; }
    public InviteLanguage Language { get; set; }
    public InviteMascot Mascot { get; set; }
}
