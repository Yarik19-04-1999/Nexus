using Information.Application.Enums;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Interfaces;

namespace Information.Application.Interfaces.UseCases;

public interface IGetExchangeRatesUseCase : ISimpleUseCase<GetExchangeRatesInput, IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>;
