namespace Lore.Api.Controllers.V1.Universes.UpdateUniverse;

public record UpdateUniverseRequest(int Id, string Name, string? Description, bool IsHidden, int ListNo);
