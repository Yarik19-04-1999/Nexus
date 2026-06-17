using Nexus.Application.Core.Utils;

namespace Nexus.Application.Core.Tests.Utils;

public class StringUtilsTests
{
    [Fact]
    public void TruncateWithEllipsis_WhenValueFitsInMaxLength_ReturnsUnchanged()
    {
        var result = StringUtils.TruncateWithEllipsis("hello", 10);

        Assert.Equal("hello", result);
    }

    [Fact]
    public void TruncateWithEllipsis_WhenValueEqualsMaxLength_ReturnsUnchanged()
    {
        var result = StringUtils.TruncateWithEllipsis("hello", 5);

        Assert.Equal("hello", result);
    }

    [Fact]
    public void TruncateWithEllipsis_WhenValueExceedsMaxLength_TruncatesWithDefaultEllipsis()
    {
        var result = StringUtils.TruncateWithEllipsis("hello world", 8);

        Assert.Equal("hello...", result);
        Assert.Equal(8, result.Length);
    }

    [Fact]
    public void TruncateWithEllipsis_WhenCustomEllipsis_TruncatesWithCustomEllipsis()
    {
        var result = StringUtils.TruncateWithEllipsis("hello world", 8, " …");

        Assert.Equal("hello w …", result);
        Assert.Equal(9, result.Length);
    }

    [Fact]
    public void TruncateWithEllipsis_WhenEllipsisLengthEqualsMaxLength_Throws()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            StringUtils.TruncateWithEllipsis("hello", 3, "..."));

        Assert.Equal("ellipsis", ex.ParamName);
    }

    [Fact]
    public void TruncateWithEllipsis_WhenEllipsisLengthExceedsMaxLength_Throws()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            StringUtils.TruncateWithEllipsis("hello", 2, "..."));

        Assert.Equal("ellipsis", ex.ParamName);
    }
}
