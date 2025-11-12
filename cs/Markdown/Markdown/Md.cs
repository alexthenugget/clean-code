using Markdown.Parsing;
using Markdown.Rendering;
using Markdown.Tokenization;

namespace Markdown;

public class Md
{
    public static string Render(string markdown)
    {
        var tokenizer = Tokenizer.CreateFor(markdown);
        var tokens = tokenizer.Tokenize(); 
        
        var parser = Parser.CreateFor(tokens);
        var root = parser.Parse();
        
        var renderer = new HtmlRenderer();
        return renderer.GetString(root);
    }
}