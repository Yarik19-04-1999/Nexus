namespace Information.Api.Controllers.V1.EpicGames.GetEpicGames;

public record EpicGameDto(
    string Title,
    string Description,
    string? ImageUrl,
    DateTimeOffset FreeUntil,
    string StoreUrl);
