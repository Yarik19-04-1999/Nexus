using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Nexus.Api.Core.Policies;

public static class NexusPolicies
{
    public static readonly AsyncRetryPolicy<HttpResponseMessage> DefaultRetryPolicy =
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
            [
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(15),
            ]);
}
