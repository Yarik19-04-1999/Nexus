using AutoFixture;

namespace Nexus.Core.Tests.Utils;

public static class FixtureUtils
{
    public static Fixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        fixture.Customize<DateOnly>(c => c.FromFactory<DateTime>(DateOnly.FromDateTime));
        return fixture;
    }
}
