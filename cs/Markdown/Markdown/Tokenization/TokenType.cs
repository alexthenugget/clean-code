namespace Markdown.Tokenization;

public enum TokenType
{
    Emphasis,
    Strong,
    Header,
    Text,
    NewLine,
    Whitespace,
    Eof,
    LeftBracket,
    RightBracket,
    LeftParen,
    RightParen
}