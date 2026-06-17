namespace Lore.Api.Controllers.V1.Universes.CreateUniverse;

public record CreateUniverseRequest(string Name, string? Description, bool IsHidden, int ListNo);
