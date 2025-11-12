using System.Text;
using Markdown.Structures;

namespace Markdown.Rendering;

public class HtmlRenderer : IRenderer
{
    private StringBuilder html;
    public string GetString(Node root)
    {
        html = new StringBuilder();
        root.Accept(this);
        return html.ToString();
    }
    
    void IRenderer.Render(TextNode node)
    {
        html.Append(node.Content);
    }

    void IRenderer.Render(RootNode node)
    {
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
    }

    void IRenderer.Render(HeaderNode node)
    {
        html.Append("<h1>");
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
        html.Append("</h1>");
    }

    void IRenderer.Render(StrongNode node)
    {
        html.Append("<strong>");
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
        html.Append("</strong>");
    }

    void IRenderer.Render(EmphasisNode node)
    {
        html.Append("<em>");
        foreach (var child in node.Children)
        {
            child.Accept(this);
        }
        html.Append("</em>");
    }
    
    void IRenderer.Render(LinkNode node)
    {
        html.Append($"<a href=\"{node.Url}\">{node.Text}</a>");
    }
}