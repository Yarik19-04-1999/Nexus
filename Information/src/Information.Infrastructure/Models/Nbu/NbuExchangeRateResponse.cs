using System.Text.Json.Serialization;

namespace Information.Infrastructure.Models.Nbu;

internal class NbuExchangeRateResponse
{
    [JsonPropertyName("cc")]
    public string Currency { get; init; } = default!;

    [JsonPropertyName("rate")]
    public decimal Rate { get; init; }

    [JsonPropertyName("exchangedate")]
    public string ExchangeDate { get; init; } = default!;
}
