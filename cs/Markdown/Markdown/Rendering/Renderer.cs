using System.Text;

namespace Markdown;

public class Renderer : IVisitor
{
    private StringBuilder html;
    public void Visit(TextNode node)
    {
        html.Append(node.Content);
    }

    public void Visit(WhitespaceNode node)
    {
        html.Append(node.Content);
    }

    public void Visit(RootNode node)
    {
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
    }

    public void Visit(HeaderNode node)
    {
        html.Append("<h1>");
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
        html.Append("</h1>");
    }

    public void Visit(StrongNode node)
    {
        html.Append("<strong>");
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
        html.Append("</strong>");
    }

    public void Visit(EmphasisNode node)
    {
        html.Append("<em>");
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
        html.Append("</em>");
    }

    public string Render(Node root)
    {
        html = new StringBuilder();
        root.Accept(this);
        return html.ToString();
    }
}