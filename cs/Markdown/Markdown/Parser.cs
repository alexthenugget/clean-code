namespace Markdown;

public class Parser
{
    private List<Token> tokens;
    private int position = 0;
    private Token CurrentToken => tokens[position];
    private List<string> html = new List<string>();
    
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public string Parse()
    {
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