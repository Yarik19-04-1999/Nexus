using Lore.Application.Models.Inputs;

namespace Lore.Api.Controllers.V1.Universes.UpdateUniverse;

public static class UpdateUniverseRequestMapper
{
    public static UpdateUniverseInput Map(UpdateUniverseRequest request)
        => new(request.Id, request.Name, request.Description, request.IsHidden, request.ListNo);
}
