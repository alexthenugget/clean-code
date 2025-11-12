using Markdown.Rendering;

namespace Markdown.Structures;

public abstract class Node
{
    public List<Node> Children { get; } = new List<Node>();
    public abstract void Accept(IRenderer visitor);
    
    public void AddChild(Node child)
    {
        Children.Add(child);
    }
}

public class RootNode : Node
{
    public override void Accept(IRenderer visitor)
    {
        visitor.Render(this);
    }
}

public class TextNode(string content) : Node
{
    public string Content { get; } = content;

    public override void Accept(IRenderer visitor)
    {
        visitor.Render(this);
    }
}

public class HeaderNode : Node
{
    public override void Accept(IRenderer visitor)
    {
        visitor.Render(this);
    }
}

public class StrongNode(bool isInvalid = false, bool isInWord = false) : Node
{
    public bool IsInvalid { get; set; } = isInvalid;
    public bool IsInWord { get; set; } = isInWord;
    public override void Accept(IRenderer visitor)
    {
        visitor.Render(this);
    }
}

public class EmphasisNode(bool isInvalid = false, bool isInWord = false) : Node
{
    public bool IsInvalid { get; set; } = isInvalid;
    public bool IsInWord { get; set; } = isInWord;

    public override void Accept(IRenderer visitor)
    {
        visitor.Render(this);
    }
}

public class LinkNode() : Node
{
    public string Text { get; set; } 
    public string Url { get; set; }

    public override void Accept(IRenderer visitor)
    {
        visitor.Render(this);
    }
}
