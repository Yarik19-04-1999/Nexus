using System.Net.Http.Json;
using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Models.EpicGames;
using Nexus.Application.Core.Models;

namespace Information.Infrastructure.Providers.EpicGames;

internal class EpicGamesProvider : IEpicGamesProvider
{
    private const string ApiUrl = "https://store-site-backend-static.ak.epicgames.com/freeGamesPromotions?locale=en-US&country=US&allowCountries=US";
    private const string StoreBaseUrl = "https://store.epicgames.com/en-US/p/";
    private const string FallbackStoreUrl = "https://store.epicgames.com/en-US/free-games";
    private const string SourceName = "EpicGames";

    private readonly HttpClient _httpClient;

    public EpicGamesProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IReadOnlyList<EpicGame>>> GetFreeGames(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<EpicGamesResponse>(ApiUrl, cancellationToken);

            if (response is null)
            {
                return InformationResultConstants.ProviderUnavailable<IReadOnlyList<EpicGame>>(SourceName);
            }

            var now = DateTimeOffset.UtcNow;

            var games = response.Data.Catalog.SearchStore.Elements
                .Where(e => IsCurrentlyFree(e, now))
                .Select(e => new EpicGame
                {
                    Title = e.Title,
                    Description = e.Description,
                    ImageUrl = GetImageUrl(e.KeyImages),
                    FreeUntil = GetFreeUntil(e, now)!.Value,
                    StoreUrl = GetStoreUrl(e),
                })
                .ToList();

            return Result<IReadOnlyList<EpicGame>>.Success(games);
        }
        catch (Exception)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyList<EpicGame>>(SourceName, canRetry: true);
        }
    }

    private static bool IsCurrentlyFree(EpicGamesElement element, DateTimeOffset now) =>
        element.Promotions?.PromotionalOffers
            .SelectMany(g => g.PromotionalOffers)
            .Any(o => o.DiscountSetting.DiscountPercentage == 0
                      && o.StartDate <= now
                      && o.EndDate >= now) is true;

    private static DateTimeOffset? GetFreeUntil(EpicGamesElement element, DateTimeOffset now) =>
        element.Promotions?.PromotionalOffers
            .SelectMany(g => g.PromotionalOffers)
            .FirstOrDefault(o => o.DiscountSetting.DiscountPercentage == 0
                                 && o.StartDate <= now
                                 && o.EndDate >= now)
            ?.EndDate;

    private static string? GetImageUrl(IReadOnlyList<EpicGamesKeyImage> keyImages) =>
        keyImages.FirstOrDefault(i => i.Type == "OfferImageWide")?.Url
        ?? keyImages.FirstOrDefault(i => i.Type == "Thumbnail")?.Url;

    private static string GetStoreUrl(EpicGamesElement element)
    {
        var slug = element.CatalogNs.Mappings?.FirstOrDefault(m => m.PageType == "productHome")?.PageSlug
                   ?? element.ProductSlug;

        return slug is not null ? $"{StoreBaseUrl}{slug}" : FallbackStoreUrl;
    }
}
