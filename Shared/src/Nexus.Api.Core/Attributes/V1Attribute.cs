using Asp.Versioning;
using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Attributes;

public class V1Attribute : ApiVersionAttribute
{
    public V1Attribute() : base(ApiVersionConstants.V1)
    {
        
    }
}
