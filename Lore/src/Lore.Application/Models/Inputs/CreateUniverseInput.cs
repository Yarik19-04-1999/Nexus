namespace Lore.Application.Models.Inputs;

public record CreateUniverseInput(string Name, string? Description, bool IsHidden, int ListNo);
