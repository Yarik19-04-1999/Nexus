using Lore.Application.Models.Inputs;

namespace Lore.Api.Controllers.V1.Universes.CreateUniverse;

public static class CreateUniverseRequestMapper
{
    public static CreateUniverseInput Map(CreateUniverseRequest request)
        => new(request.Name, request.Description, request.IsHidden, request.ListNo);
}
