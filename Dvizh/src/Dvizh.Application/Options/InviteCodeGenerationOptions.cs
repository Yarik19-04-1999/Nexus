namespace Dvizh.Application.Options;

public class InviteCodeGenerationOptions
{
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
}
