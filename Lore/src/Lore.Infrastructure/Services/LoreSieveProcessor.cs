using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Lore.Infrastructure.Services;

public class LoreSieveProcessor : SieveProcessor
{
    public LoreSieveProcessor(IOptions<SieveOptions> options) : base(options)
    {
    }
}
