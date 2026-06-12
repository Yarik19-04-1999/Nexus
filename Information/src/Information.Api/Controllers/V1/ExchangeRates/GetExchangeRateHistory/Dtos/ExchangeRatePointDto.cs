namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory.Dtos;

public record ExchangeRatePointDto(decimal Rate, DateOnly Date);
