namespace Markdown;

public class Md
{
    private Tokenizer tokenizer;
    private Parser parser;
    private Renderer renderer;
    
    public Md()
    {
        tokenizer = new Tokenizer();
        parser = new Parser();
        renderer = new Renderer();
    }

    public string Render(string markdown)
    {
        List<Token> tokens = tokenizer.Tokenize(markdown);
        Node root = parser.Parse(tokens);
        return renderer.Render(root);
    }
}