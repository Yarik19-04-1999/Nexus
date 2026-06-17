using System.Net.Http.Json;
using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Models.EpicGames;
using Nexus.Application.Core.Exceptions;

namespace Information.Infrastructure.Providers.EpicGames;

internal class EpicGamesProvider : IEpicGamesProvider
{
    private const string StoreBaseUrl = "https://store.epicgames.com/en-US/p/";
    private const string FallbackStoreUrl = "https://store.epicgames.com/en-US/free-games";
    private const string SourceName = "EpicGames";
    private const string GraphQlEndpoint = "/graphql";

    private static readonly object Query = new
    {
        query = """
            query getFreeGames($locale: String, $country: String, $allowCountries: String) {
              Catalog {
                searchStore(
                  locale: $locale
                  country: $country
                  allowCountries: $allowCountries
                  category: "freegames"
                  count: 10
                ) {
                  elements {
                    title
                    description
                    keyImages { type url }
                    productSlug
                    catalogNs {
                      mappings(pageType: "productHome") {
                        pageSlug
                        pageType
                      }
                    }
                    promotions(category: "freegames") {
                      promotionalOffers {
                        promotionalOffers {
                          startDate
                          endDate
                          discountSetting { discountPercentage }
                        }
                      }
                    }
                  }
                }
              }
            }
            """,
        variables = new
        {
            locale = "en-US",
            country = "US",
            allowCountries = "US"
        }
    };

    private readonly HttpClient _httpClient;

    public EpicGamesProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<EpicGame>> GetFreeGames(CancellationToken cancellationToken = default)
    {
        try
        {
            var httpResponse = await _httpClient.PostAsJsonAsync(GraphQlEndpoint, Query, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<EpicGamesResponse>(cancellationToken);

            if (response?.Data is null)
            {
                throw InformationExceptions.ProviderUnavailable(SourceName);
            }

            var now = DateTimeOffset.UtcNow;

            return response.Data.Catalog.SearchStore.Elements
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
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw InformationExceptions.ProviderUnavailable(SourceName);
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
