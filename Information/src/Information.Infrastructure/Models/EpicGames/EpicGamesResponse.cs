using System.Text.Json.Serialization;

namespace Information.Infrastructure.Models.EpicGames;

internal class EpicGamesResponse
{
    [JsonPropertyName("data")]
    public EpicGamesData Data { get; init; } = default!;
}

internal class EpicGamesData
{
    [JsonPropertyName("Catalog")]
    public EpicGamesCatalog Catalog { get; init; } = default!;
}

internal class EpicGamesCatalog
{
    [JsonPropertyName("searchStore")]
    public EpicGamesSearchStore SearchStore { get; init; } = default!;
}

internal class EpicGamesSearchStore
{
    [JsonPropertyName("elements")]
    public IReadOnlyList<EpicGamesElement> Elements { get; init; } = default!;
}

internal class EpicGamesElement
{
    [JsonPropertyName("title")]
    public string Title { get; init; } = default!;

    [JsonPropertyName("description")]
    public string Description { get; init; } = default!;

    [JsonPropertyName("keyImages")]
    public IReadOnlyList<EpicGamesKeyImage> KeyImages { get; init; } = default!;

    [JsonPropertyName("productSlug")]
    public string? ProductSlug { get; init; }

    [JsonPropertyName("catalogNs")]
    public EpicGamesCatalogNs CatalogNs { get; init; } = default!;

    [JsonPropertyName("promotions")]
    public EpicGamesPromotions? Promotions { get; init; }
}

internal class EpicGamesKeyImage
{
    [JsonPropertyName("type")]
    public string Type { get; init; } = default!;

    [JsonPropertyName("url")]
    public string Url { get; init; } = default!;
}

internal class EpicGamesCatalogNs
{
    [JsonPropertyName("mappings")]
    public IReadOnlyList<EpicGamesMapping>? Mappings { get; init; }
}

internal class EpicGamesMapping
{
    [JsonPropertyName("pageSlug")]
    public string PageSlug { get; init; } = default!;

    [JsonPropertyName("pageType")]
    public string PageType { get; init; } = default!;
}

internal class EpicGamesPromotions
{
    [JsonPropertyName("promotionalOffers")]
    public IReadOnlyList<EpicGamesOfferGroup> PromotionalOffers { get; init; } = default!;
}

internal class EpicGamesOfferGroup
{
    [JsonPropertyName("promotionalOffers")]
    public IReadOnlyList<EpicGamesOffer> PromotionalOffers { get; init; } = default!;
}

internal class EpicGamesOffer
{
    [JsonPropertyName("startDate")]
    public DateTimeOffset StartDate { get; init; }

    [JsonPropertyName("endDate")]
    public DateTimeOffset EndDate { get; init; }

    [JsonPropertyName("discountSetting")]
    public EpicGamesDiscountSetting DiscountSetting { get; init; } = default!;
}

internal class EpicGamesDiscountSetting
{
    [JsonPropertyName("discountPercentage")]
    public int DiscountPercentage { get; init; }
}
