using Information.Application.Enums;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.UseCases;

public interface IGetExchangeRatesUseCase : IUseCase<GetExchangeRatesInput, Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>>;
