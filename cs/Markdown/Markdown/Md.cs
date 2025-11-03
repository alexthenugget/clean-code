namespace Markdown;

public class Md
{
    private readonly Tokenizer tokenizer;
    private readonly HtmlRenderer renderer;

    public Md()
    {
        tokenizer = new Tokenizer();
        renderer = new HtmlRenderer();
    }

    public string Render(string markdown)
    {
        var tokens = tokenizer.Tokenize(markdown);
        return renderer.RenderHtml(tokens);
    }
}