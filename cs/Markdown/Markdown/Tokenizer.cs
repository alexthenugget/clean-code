using System.Text;

namespace Markdown;

public class Tokenizer
{
    private readonly string markdown;
    private int position = 0;
    
    public Tokenizer(string markdown)
    {
        this.markdown = markdown;
    }
    
    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();
        while (position < markdown.Length)
        {
            tokens.Add(GetNextToken());
        }
        tokens.Add(new Token(TokenType.Eof, "\0"));
        return tokens;
    }

    private Token GetNextToken()
    {
        return new Token(TokenType.Text, "text");
    }
}