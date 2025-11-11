namespace Markdown;

using FluentAssertions;
using NUnit.Framework;

public class MarkdownTests
{
    [TestCaseSource(typeof(MarkdownTestData), nameof(MarkdownTestData.AllTestCases))]
    public void Markdown_Render_ReturnsExpectedResult(string input, string expected)
    {
        var md = new Md();
        var result = md.Render(input);
        result.Should().Be(expected);
    }
}