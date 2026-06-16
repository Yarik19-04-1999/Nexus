using Dvizh.Application.Enums;
using Sieve.Models;

namespace Dvizh.Application.Models.Input;

public record GetInvitesInput(SieveModel SieveModel, ExpiryFilter ExpiryFilter = ExpiryFilter.All);
