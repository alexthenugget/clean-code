namespace Markdown;

public enum TokenType
{
    Emphasis,
    Strong,
    Header,
    Text,
    NewLine,
    Whitespace,
    EscapedChar,
    Eof,
    Ref
}