using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Markdown.Tests;

public class MarkdownTests
{
    [Test]
    public void Performance_ShouldBeLinear()
    {
        const int repetitions = 50000;
        const string inputFragment = "_text_ __text__ # text\n";
        
        var largeInput = new StringBuilder();
        for (int i = 0; i < repetitions; i++)
        {
            largeInput.Append(inputFragment);
        }
        var stopwatch = new Stopwatch();

        stopwatch.Start();
        
        var md = new Md();
        var result = Md.Render(largeInput.ToString());
        
        stopwatch.Stop();
        
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(500);
    }
    
    [TestCaseSource(typeof(MarkdownTestData), nameof(MarkdownTestData.AllTestCases))]
    public void Markdown_Render_ReturnsExpectedResult(string input, string expected)
    {
        var result = Md.Render(input);
        result.Should().Be(expected);
    }
}