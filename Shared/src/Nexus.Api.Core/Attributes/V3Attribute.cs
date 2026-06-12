using Asp.Versioning;
using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Attributes;

public class V3Attribute : ApiVersionAttribute
{
    public V3Attribute() : base(ApiVersionConstants.V3)
    {
        
    }
}
