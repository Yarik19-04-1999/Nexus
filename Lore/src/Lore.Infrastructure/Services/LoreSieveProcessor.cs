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

        mapper.Property<Movie>(x => x.Title).CanFilter().CanSort();
        mapper.Property<Movie>(x => x.ReleaseYear).CanFilter().CanSort();
        mapper.Property<Movie>(x => x.Score).CanFilter().CanSort();
        mapper.Property<Movie>(x => x.RewatchStatus).CanFilter().CanSort();
        mapper.Property<Movie>(x => x.UniverseId).CanFilter().CanSort();
        mapper.Property<Movie>(x => x.ListNo).CanSort();

        return mapper;
    }
}
