using Dvizh.Application.Models;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Dvizh.Application.Services;

public class DvizhSieveProcessor : SieveProcessor
{
    public DvizhSieveProcessor(IOptions<SieveOptions> options) : base(options)
    {
    }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.Property<Invite>(x => x.Answer).CanFilter().CanSort();
        mapper.Property<Invite>(x => x.Message).CanFilter();
        mapper.Property<Invite>(x => x.ExpiresAt).CanFilter().CanSort();
        mapper.Property<Invite>(x => x.CreatedAt).CanSort();
        mapper.Property<Invite>(x => x.UpdatedAt).CanSort();

        mapper.Property<InviteEvent>(x => x.EventType).CanFilter().CanSort();
        mapper.Property<InviteEvent>(x => x.CreatedAt).CanSort();

        return mapper;
    }
}
