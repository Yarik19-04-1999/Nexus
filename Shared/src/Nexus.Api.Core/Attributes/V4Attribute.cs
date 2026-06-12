using Asp.Versioning;
using Nexus.Api.Core.Constants;

namespace Nexus.Api.Core.Attributes;

public class V4Attribute : ApiVersionAttribute
{
    public V4Attribute() : base(ApiVersionConstants.V4)
    {
        
    }
}
