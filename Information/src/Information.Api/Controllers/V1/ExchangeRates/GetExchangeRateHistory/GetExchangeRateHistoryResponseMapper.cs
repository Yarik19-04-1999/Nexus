using Information.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetExchangeRateHistoryResponseMapper
{
    public static partial IReadOnlyList<GetExchangeRateHistoryResponse> Map(IReadOnlyList<ExchangeRateHistory> histories);
}
