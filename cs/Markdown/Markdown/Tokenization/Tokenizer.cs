namespace Markdown.Tokenization;

public class Tokenizer
{
    private readonly string markdown;
    private int position;
    
    private static readonly HashSet<char> SpecialCharacters = new()
    {
        '\\',
        ' ',
        '_',
        '#',
        '\n',
        '[',
        ']',
        '(',
        ')'
    };
    
    char CurrentChar => markdown[position];
    char NextChar => position + 1 < markdown.Length ? markdown[position + 1] : '\0';
    char PrevChar => position > 0 ? markdown[position - 1] : '\0';

    private Tokenizer(string markdown)
    {
        this.markdown = markdown;
        position = 0;
    }

    public static Tokenizer CreateFor(string markdown) => new (markdown);

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
        switch (CurrentChar)
        {
            case '\\':
                return HandleEscapedChar();
            case ' ':
                return HandleWhitespace();
            case '_' when NextChar == '_':
                return HandleDoubleUnderscore();
            case '_':
                return HandleSingleUnderscore();
            case '#' when NextChar == ' ':
                return HandleHeader();
            case '\n':
                return HandleNewLine();
            case '[':
                position++;
                return new Token(TokenType.LeftBracket, "[");
            case ']':
                position++;
                return new Token(TokenType.RightBracket, "]");
            case '(':
                position++;
                return new Token(TokenType.LeftParen, "(");
            case ')':
                position++;
                return new Token(TokenType.RightParen, ")");
            default:
                return HandleText();
        }
    }

    private Token HandleWhitespace()
    {
        position++;
        return new Token(TokenType.Whitespace, " ");
    }
    
    private Token HandleDoubleUnderscore()
    {
        var next = (position + 2 < markdown.Length) ? markdown[position + 2] : '\0';
        var afterNext = (position + 3 < markdown.Length) ? markdown[position + 3] : '\0';
    
        if (next == '_' && afterNext == '_')
        {
            position += 4;
            return new Token(TokenType.Text, "____");
        }
        
        var charBefore = PrevChar;
        
        if (!char.IsWhiteSpace(next) && !char.IsWhiteSpace(charBefore))
        {
            if (char.IsDigit(next) || char.IsDigit(charBefore))
            {
                position+=2;
                return new Token(TokenType.Text, "__");
            }
            position+=2;
            return new Token(TokenType.Strong, "__");
        }

        if (char.IsWhiteSpace(next) && char.IsWhiteSpace(charBefore))
        {
            position+=2;
            return new Token(TokenType.Strong, "__", true, true);
        }
        if (char.IsWhiteSpace(next))
        {
            position+=2;
            return new Token(TokenType.Strong, "__", false, true);
        }
        position+=2;
        return new Token(TokenType.Strong, "__", true, false);
    }

    private Token HandleSingleUnderscore()
    {
        try
        {
            var charAfter = NextChar;
            var charBefore = PrevChar;

            if (!char.IsWhiteSpace(charAfter) && !char.IsWhiteSpace(charBefore))
            {
                if (char.IsDigit(charAfter) || char.IsDigit(charBefore))
                    return new Token(TokenType.Text, "_");

                if (charBefore == '\\')
                    return new Token(TokenType.Emphasis, "_", true, false);

                return new Token(TokenType.Emphasis, "_");
            }

            if (char.IsWhiteSpace(charAfter) && char.IsWhiteSpace(charBefore))
                return new Token(TokenType.Emphasis, "_", true, true);

            if (char.IsWhiteSpace(charAfter))
                return new Token(TokenType.Emphasis, "_", false, true);

            return new Token(TokenType.Emphasis, "_", true, false);
        }
        finally
        {
            position++;
        }
    }

    private Token HandleEscapedChar()
    {
        if (NextChar == '_')
        {
            position += 2;
            return new Token(TokenType.Text, "_");
        }

        if (NextChar == '\\')
        {
            position += 2;
            return new Token(TokenType.Text, "\\");
        }
        
        if (NextChar == '#')
        {
            position += 2;
            return new Token(TokenType.Text, "#");
        }
        
        if (NextChar == '[')
        {
            position += 2;
            return new Token(TokenType.Text, "[");
        }
        
        if (NextChar == ']')
        {
            position += 2;
            return new Token(TokenType.Text, "]");
        }
        
        if (NextChar == '(')
        {
            position += 2;
            return new Token(TokenType.Text, "(");
        }
        
        if (NextChar == ')')
        {
            position += 2;
            return new Token(TokenType.Text, ")");
        }
        position++;
        return new Token(TokenType.Text, "\\");
    }

    private Token HandleHeader()
    {
        position += 2;
        return new Token(TokenType.Header, "# ");
    }

    private Token HandleNewLine()
    {
        position++;
        return new Token(TokenType.NewLine, "\n");
    }

    private Token HandleText()
    {
        var startPosition = position;
        position++;
        while (position < markdown.Length && !SpecialCharacters.Contains(CurrentChar))
        {
            position++;
        }

        var textValue = markdown.Substring(startPosition, position - startPosition);
        return new Token(TokenType.Text, textValue);
    }
}