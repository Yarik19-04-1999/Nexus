namespace Dvizh.Application.Options;

public class UniqueCodeServiceOptions
{
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);
}
