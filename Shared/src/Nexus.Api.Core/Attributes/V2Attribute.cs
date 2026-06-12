using Asp.Versioning;
using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Attributes;

public class V2Attribute : ApiVersionAttribute
{
    public V2Attribute() : base(ApiVersionConstants.V2)
    {
        
    }
}
