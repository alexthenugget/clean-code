namespace Markdown;

public class Md
{
    private Tokenizer tokenizer;
    private Renderer renderer;
    
    public Md()
    {
        tokenizer = new Tokenizer();
        renderer = new Renderer();
    }

    public string Render(string markdown)
    {
        List<Token> tokens = tokenizer.Tokenize(markdown);
        return renderer.Render(tokens);
    }
}