namespace Markdown;

public class Renderer
{
    private List<Token> tokens;
    private int position = 0;
    private Token CurrentToken => tokens[position];
    private List<string> html = new List<string>();

    public string Render(List<Token> tokens)
    {
        this.tokens = tokens;
        while (CurrentToken.Type != TokenType.Eof)
        {
            ConvertToHtml();
            position++;
        }
        return string.Join("", html);
    }

    private void ConvertToHtml()
    {
    }
}