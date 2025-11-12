namespace Markdown.Tokenization;

public class Token(TokenType type, string value, bool isAfterWhitespace = false, bool isBeforeWhitespace = false)
{
    public TokenType Type { get; } = type;
    public string Value { get; } = value;
    public bool IsAfterWhitespace { get; } = isAfterWhitespace;
    public bool IsBeforeWhitespace { get; } = isBeforeWhitespace;

    public override string ToString()
    {
        return Value;
    }
}