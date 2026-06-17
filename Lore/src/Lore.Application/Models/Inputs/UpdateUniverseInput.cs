namespace Lore.Application.Models.Inputs;

public record UpdateUniverseInput(int Id, string Name, string? Description, bool IsHidden, int ListNo);
