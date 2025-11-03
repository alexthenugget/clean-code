namespace Markdown;

public class Token(TokenType type, string value, int position)
{
    public int Length { get; } = value.Length;
    public int Position { get; } = position;
    public string Value { get; } = value;
    public TokenType Type { get; } = type;
}