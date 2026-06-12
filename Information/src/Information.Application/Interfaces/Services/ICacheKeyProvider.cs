namespace Information.Application.Interfaces.Services;

public interface ICacheKeyProvider
{
    string ExchangeRates(DateOnly date);
}
