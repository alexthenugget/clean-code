namespace Markdown;

using FluentAssertions;
using NUnit.Framework;

public class MarkdownTests
{
    [TestCase("Simple text", "Simple text", TestName = "Тест без спец символов")]
    [TestCase("_Simple text_", "<em>Simple text</em>", TestName = "Простой тест курсив")]
    [TestCase("__Simple text__", "<strong>Simple text</strong>", TestName = "Простой тест полужирный")]
    public void Markdown_Render_ReturnsExpectedResult(string input, string expected)
    {
        var md = new Md();
        var result = md.Render(input);
        result.Should().Be(expected);
    }
}