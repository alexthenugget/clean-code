namespace Markdown;

public interface IVisitor
{
    void Visit(RootNode node);
    void Visit(TextNode node);
    void Visit(StrongNode node);
    void Visit(EmphasisNode node);
    void Visit(HeaderNode node);
    void Visit(WhitespaceNode node);
}