using Nexus.Api.Core.ViewModels;
using Nexus.Application.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Nexus.Api.Core.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ResponsesMappers
{
    public static partial DomainErrorResponse MapToDomainErrorResponse(this Result domainError);
}
