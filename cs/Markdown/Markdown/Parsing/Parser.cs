namespace Markdown;

public class Parser
{
    private Stack<Node> stack;
    private List<Token> tokens;
    private Node root;
    private int position;
    private Token CurrentToken => tokens[position];
    private Token NextToken => position + 1 < tokens.Count ? tokens[position + 1] : new Token(TokenType.Eof, "\0");
    
    public Node Parse(List<Token> tokens)
    {
        this.tokens = tokens;
        this.position = 0;
        this.root = new RootNode();
        this.stack = new Stack<Node>();
        
        stack.Push(root);
        
        while (CurrentToken.Type != TokenType.Eof)
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Header:
                    HandleHeader();
                    break;
                case TokenType.NewLine:
                    HandleNewLine();
                    break;
                case TokenType.Strong:
                    HandleStrong();
                    break;
                case TokenType.Emphasis:
                    HandleEmphasis();
                    break;
                case TokenType.EscapedChar:
                    HandleEscapedChar();
                    break;
                default:
                    HandleText();
                    break;
            }
            position++;
        }
        CloseUnusedMarkers();
        return root;
    }
    
    private void HandleHeader()
    {
        var headerNode = new HeaderNode();
        stack.Peek().AddChild(headerNode);
        stack.Push(headerNode);
    }

    private void HandleNewLine()
    {
        var current = stack.Peek();
        if (current is HeaderNode)
        {
            stack.Pop();
            return;
        }
        current.AddChild(new TextNode("\n"));
    }

    private void HandleStrong()
    {
        var current = stack.Peek();
        if (current is EmphasisNode)
        {
            current.AddChild(new TextNode(CurrentToken.Value));
            return;
        }
        if (current is StrongNode)
        {
            stack.Pop();
            return;
        }
        var strongNode = new StrongNode();
        current.AddChild(strongNode);
        stack.Push(strongNode);
    }

    private void HandleEmphasis()
    {
        var current = stack.Peek();
        if (current is EmphasisNode)
        {
            stack.Pop();
            return;
        }
        var emphasisNode = new EmphasisNode();
        current.AddChild(emphasisNode);
        stack.Push(emphasisNode);
    }

    private void HandleEscapedChar()
    {
        var current = stack.Peek();
        if (NextToken.Type is TokenType.Emphasis or TokenType.EscapedChar)
        {
            current.AddChild(new TextNode(NextToken.Value));
            position++;
        }
        else
        {
            current.AddChild(new TextNode(CurrentToken.Value));
        }
    }

    private void HandleText()
    {
        var current = stack.Peek();
        current.AddChild(new TextNode(CurrentToken.Value));
    }
    
    private void CloseUnusedMarkers()
    {
        while (stack.Count > 1)
        {
            var unclosedNode = stack.Pop();
            
            if (unclosedNode is HeaderNode)
            {
                continue;
            }
            
            var parent = stack.Peek();
            var nodeIndex = parent.Children.IndexOf(unclosedNode);
            parent.Children.RemoveAt(nodeIndex);
            
            var textMarkerNode = unclosedNode is EmphasisNode ? new TextNode("_") : new TextNode("__");
            
            var insertIndex = nodeIndex;
            parent.Children.Insert(insertIndex, textMarkerNode);
            insertIndex++;
            
            if (unclosedNode.Children.Count > 0)
            {
                parent.Children.InsertRange(insertIndex, unclosedNode.Children);
            }
        }
    }
}