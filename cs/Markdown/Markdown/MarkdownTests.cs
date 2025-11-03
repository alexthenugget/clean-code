namespace Markdown;

using FluentAssertions;
using NUnit.Framework;

public class MarkdownTests
{
    [Test]
    public void Markdown_SimpleText()
    {
        var md = new Md();
        var result = md.Render("Simple text");
        result.Should().Be("Simple text");
    }
    [Test]
    public void Markdown_SimpleEmphasisText()
    {
        var md = new Md();
        var result = md.Render("_Simple text_");
        result.Should().Be("<em>Simple text</em>");
    }
    [Test]
    public void Markdown_SimpleStrongText()
    {
        var md = new Md();
        var result = md.Render("__Simple text__");
        result.Should().Be("<strong>Simple text</strong>");
    }
}