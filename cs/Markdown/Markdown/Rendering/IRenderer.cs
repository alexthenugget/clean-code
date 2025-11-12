using Markdown.Structures;

namespace Markdown.Rendering;

public interface IRenderer
{
    void Render(RootNode node);
    void Render(TextNode node);
    void Render(StrongNode node);
    void Render(EmphasisNode node);
    void Render(HeaderNode node);
    void Render(LinkNode node);
}