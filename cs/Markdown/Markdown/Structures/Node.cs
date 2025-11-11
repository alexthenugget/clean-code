namespace Markdown;

public abstract class Node
{
    public List<Node> Children { get; } = new List<Node>();
    public abstract void Accept(IVisitor visitor);
    
    public void AddChild(Node child)
    {
        Children.Add(child);
    }
}

public class RootNode : Node
{
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class TextNode(string content) : Node
{
    public string Content { get; } = content;

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class HeaderNode : Node
{
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class StrongNode : Node
{
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class EmphasisNode : Node
{
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class WhitespaceNode(string content) : Node
{
    public string Content { get; } = content;
    
    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}