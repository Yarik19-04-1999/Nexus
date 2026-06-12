using Information.Application.Enums;

namespace Information.Application.Models.Input;

public record GetExchangeRateHistoryInput(ExchangeCurrency? Currency);
