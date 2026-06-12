using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.UseCases;

public interface IGetExchangeRateHistoryUseCase : IUseCase<GetExchangeRateHistoryInput, Result<IReadOnlyList<ExchangeRateHistory>>>;
