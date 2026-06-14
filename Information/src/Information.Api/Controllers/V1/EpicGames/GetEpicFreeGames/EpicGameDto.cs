namespace Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;

public record EpicGameDto(
    string Title,
    string Description,
    string? ImageUrl,
    DateTimeOffset FreeUntil,
    string StoreUrl);
