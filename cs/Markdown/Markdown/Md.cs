namespace Markdown;

public class Md
{
    private Tokenizer tokenizer;
    private Parser parser;

    public string Render(string markdown)
    {
        tokenizer = new Tokenizer(markdown);
        parser = new Parser(tokenizer.Tokenize());
        return parser.Parse();
    }
}