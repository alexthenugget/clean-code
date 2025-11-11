using System.Text;

namespace Markdown;

public class Tokenizer
{
    private string markdown;
    private int position;
    private char CurrentChar => markdown[position];
    private char NextChar => position + 1 < markdown.Length ? markdown[position + 1] : '\0';
    private char PrevChar => position > 0 ? markdown[position - 1] : '\0';
    
    public List<Token> Tokenize(string markdown)
    {
        this.markdown = markdown;
        this.position = 0;
        
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
        var char3 = (position + 2 < markdown.Length) ? markdown[position + 2] : '\0';
        var char4 = (position + 3 < markdown.Length) ? markdown[position + 3] : '\0';
    
        if (char3 == '_' && char4 == '_')
        {
            position += 4;
            return new Token(TokenType.Text, "____");
        }

        var charAfter = char3;
            
        var isAdjacentToDigitStrong = char.IsDigit(PrevChar) || char.IsDigit(charAfter);
        if (isAdjacentToDigitStrong)
        {
            position += 2;
            return new Token(TokenType.Text, "__");
        }

        bool canBeOpenerEm = !char.IsWhiteSpace(charAfter) && charAfter != '\0';
        bool canBeCloserEm = !char.IsWhiteSpace(PrevChar) && PrevChar != '\0';

        if (!canBeOpenerEm && !canBeCloserEm)
        {
            position += 2;
            return new Token(TokenType.Text, "__");
        }
            
        position += 2;
        return new Token(TokenType.Strong, "__");
        
    }

    private Token HandleSingleUnderscore()
    {
        var charAfterSingle = NextChar;
        var charBefore = PrevChar;
        
        var isAdjacentToDigitEmphasis = char.IsDigit(charBefore) || char.IsDigit(charAfterSingle);
        if (isAdjacentToDigitEmphasis)
        {
            position++;
            return new Token(TokenType.Text, "_");
        }

        if (char.IsLetterOrDigit(charBefore) && char.IsLetterOrDigit(charAfterSingle))
        {
            position++;
            return new Token(TokenType.Text, "_");
        }

        bool canBeOpener = !char.IsWhiteSpace(charAfterSingle) && charAfterSingle != '\0';
        bool canBeCloser = !char.IsWhiteSpace(charBefore) && charBefore != '\0';

        if (!canBeOpener && !canBeCloser)
        {
            position++;
            return new Token(TokenType.Text, "_");
        }
        position++;
        return new Token(TokenType.Emphasis, "_");
    }

    private Token HandleEscapedChar()
    {
        position++;
        return new Token(TokenType.EscapedChar, "\\");
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
        var stringBuilder = new StringBuilder();
        while (position < markdown.Length 
               && CurrentChar != '_' 
               && CurrentChar != '#' 
               && CurrentChar != '\n' 
               && CurrentChar != '\\'
               && CurrentChar != ' ')
        {
            stringBuilder.Append(CurrentChar);
            position++;
        }
        return new Token(TokenType.Text, stringBuilder.ToString());
    }
}