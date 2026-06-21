using Lore.Application.Models;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Lore.Infrastructure.Services;

public class LoreSieveProcessor : SieveProcessor
{
    public LoreSieveProcessor(IOptions<SieveOptions> options) : base(options)
    {
    }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<Universe>(x => x.Name).CanFilter().CanSort();
        mapper.Property<Universe>(x => x.ListNo).CanSort();

        return mapper;
    }
}
