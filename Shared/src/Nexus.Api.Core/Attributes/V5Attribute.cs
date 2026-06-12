using Asp.Versioning;
using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Attributes;

public class V5Attribute : ApiVersionAttribute
{
    public V5Attribute() : base(ApiVersionConstants.V5)
    {
        
    }
}
