using Markdown.Structures;
using Markdown.Tokenization;

namespace Markdown.Parsing;

public class Parser
{
    private readonly List<Token> tokens;
    private int position;
    
    Token CurrentToken => tokens[position];

    private Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        position = 0;
    }
    
    public static Parser CreateFor(List<Token> tokens) => new (tokens);
    
    public Node Parse()
    {
        var root = new RootNode();
        var stack = new Stack<Node>();
        stack.Push(root);
        while (position < tokens.Count && tokens[position].Type != TokenType.Eof)
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Header:
                    HandleHeader(stack);
                    break;
                case TokenType.NewLine:
                    HandleNewLine(stack);
                    break;
                case TokenType.Strong:
                    HandleStrong(stack);
                    break;
                case TokenType.Emphasis:
                    HandleEmphasis(stack);
                    break;
                case TokenType.Whitespace:
                    HandleWhitespace(stack);
                    break;
                case TokenType.LeftBracket:
                    HandleLeftBracket(stack);
                    break;
                default:
                    HandleText(stack);
                    break;
            }
            position++;
        }
        CloseUnusedMarkers(stack);
        return root;
    }
    
    private void HandleLeftBracket(Stack<Node> stack)
    {
        var textTokens = new List<Token>();
        var bracketPosition = position + 1;
    
        while (bracketPosition < tokens.Count && tokens[bracketPosition].Type != TokenType.RightBracket)
        {
            if (tokens[bracketPosition].Type == TokenType.NewLine)
            {
                HandleText(stack);
                return;
            }
            textTokens.Add(tokens[bracketPosition]);
            bracketPosition++;
        }
        if (bracketPosition == tokens.Count)
        {
            HandleText(stack);
            return;
        }
        var parenPosition = bracketPosition + 1;
        if (parenPosition >= tokens.Count || tokens[parenPosition].Type != TokenType.LeftParen)
        {
            HandleText(stack);
            return;
        }
        var urlTokens = new List<Token>();
        var endPosition = parenPosition + 1;

        while (endPosition < tokens.Count && tokens[endPosition].Type != TokenType.RightParen)
        {
            if (tokens[endPosition].Type == TokenType.NewLine)
            {
                HandleText(stack);
                return;
            }
            urlTokens.Add(tokens[endPosition]);
            endPosition++;
        }

        if (endPosition == tokens.Count)
        {
            HandleText(stack);
            return;
        }
    
        var linkText = string.Concat(textTokens.Select(t => t.Value));
        var linkUrl = string.Concat(urlTokens.Select(t => t.Value));

        if (string.IsNullOrEmpty(linkText) || string.IsNullOrEmpty(linkUrl))
        {
            HandleText(stack);
            return;
        }
    
        var linkNode = new LinkNode
        {
            Text = linkText,
            Url = linkUrl
        };
        stack.Peek().AddChild(linkNode);
        position = endPosition; 
    }

    private void HandleWhitespace(Stack<Node> stack)
    {
        var currentNode = stack.Peek();
        
        if (currentNode is EmphasisNode emphasisNode && emphasisNode.IsInWord)
        {
            emphasisNode.IsInvalid = true;
        }

        currentNode.AddChild(new TextNode(CurrentToken.Value));
    }

    private void HandleHeader(Stack<Node> stack)
    {
        var currentNode = stack.Peek();

        if (currentNode is HeaderNode)
        {
            currentNode.AddChild(new TextNode("# "));
        }
        else
        {
            var headerNode = new HeaderNode();
            currentNode.AddChild(headerNode);
            stack.Push(headerNode);
        }
    }

    private void HandleNewLine(Stack<Node> stack)
    {
        var currentNode = stack.Peek();
        if (currentNode is HeaderNode)
        {
            stack.Pop();
        }
        stack.Peek().AddChild(new TextNode("\n"));
    }

    private void HandleStrong(Stack<Node> stack)
    {
        var currentNode = stack.Peek();

        if (currentNode is StrongNode)
        {
            HandleClosingStrong(stack);
        }
        else
        {
            HandleOpeningStrong(stack);
        }
    }

    private void HandleOpeningStrong(Stack<Node> stack)
    {
        var currentNode = stack.Peek();
        if (currentNode is EmphasisNode emphasisNode)
        {
            bool isCrossing = stack.Skip(1).Any(n => n is StrongNode);
            if (isCrossing)
            {
                emphasisNode.IsInvalid = true;
            }
            currentNode.AddChild(new TextNode("__"));
            return;
        }
        if (CurrentToken.IsBeforeWhitespace)
        {
            currentNode.AddChild(new TextNode("__"));
            return;
        }
        
        var strongNodeNew = new StrongNode();
        if (!CurrentToken.IsBeforeWhitespace && !CurrentToken.IsAfterWhitespace)
        {
            strongNodeNew.IsInWord = true;
        }
        stack.Push(strongNodeNew);
    }
    
    private void HandleClosingStrong(Stack<Node> stack)
    {
        var currentNode = (StrongNode)stack.Peek();

        if (currentNode.IsInvalid)
        {
            currentNode.AddChild(new TextNode("__"));
            return;
        }
        if (CurrentToken.IsAfterWhitespace)
        {
            currentNode.AddChild(new TextNode("__"));
            return;
        }
        if (currentNode.Children.Count == 0)
        {
            currentNode.AddChild(new TextNode("__"));
            return;
        }
        var closedNode = stack.Pop();
        stack.Peek().AddChild(closedNode);
    }

    private void HandleEmphasis(Stack<Node> stack)
    {
        var currentNode = stack.Peek();

        if (currentNode is EmphasisNode)
        {
            HandleClosingEmphasis(stack);
        }
        else
        {
            HandleOpeningEmphasis(stack);
        }
    }

    private void HandleClosingEmphasis(Stack<Node> stack)
    {
        var currentNode = (EmphasisNode)stack.Peek();
            
        if (currentNode.IsInvalid)
        {
            currentNode.AddChild(new TextNode("_"));
            return;
        }
        if (CurrentToken.IsAfterWhitespace)
        {
            currentNode.AddChild(new TextNode("_"));
            return;
        }
        if (currentNode.Children.Count == 0)
        {
            currentNode.AddChild(new TextNode("_"));
            return;
        }
        var closedNode = stack.Pop();
        stack.Peek().AddChild(closedNode);
    }

    private void HandleOpeningEmphasis(Stack<Node> stack)
    {
        var currentNode = stack.Peek();
        if (currentNode is StrongNode strongNode)
        {
            var isCrossing = stack.Skip(1).Any(n => n is EmphasisNode);
            if (isCrossing)
            {
                strongNode.IsInvalid = true;
                currentNode.AddChild(new TextNode("_"));
                return;
            }
        }
        if (CurrentToken.IsBeforeWhitespace)
        {
            currentNode.AddChild(new TextNode("_"));
            return;
        }
        var emphasisNode = new EmphasisNode();
        
        if (!CurrentToken.IsBeforeWhitespace && !CurrentToken.IsAfterWhitespace)
        {
            emphasisNode.IsInWord = true;
        }
        stack.Push(emphasisNode);
    }

    private void HandleText(Stack<Node> stack)
    {
        var currentNode = stack.Peek();
        currentNode.AddChild(new TextNode(CurrentToken.Value));
    }
    
    private void CloseUnusedMarkers(Stack<Node> stack)
    {
        while (stack.Count > 1)
        {
            var unclosedNode = stack.Pop();
        
            if (unclosedNode is HeaderNode)
            {
                continue;
            }
        
            var parent = stack.Peek();
            var textMarkerNode = unclosedNode is EmphasisNode ? new TextNode("_") : new TextNode("__");
            parent.AddChild(textMarkerNode);

            if (unclosedNode.Children.Count > 0)
            {
                parent.Children.AddRange(unclosedNode.Children);
            }
        }
    }
}