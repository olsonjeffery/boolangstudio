using System.Collections.Generic;

namespace Boo.BooLangService.Document.Nodes
{
    public interface IBooParseTreeNode
    {
        IBooParseTreeNode Parent { get; set; }
        IList<IBooParseTreeNode> Children { get; }
        string Name { get; set; }
        int StartLine { get; set; }
        int EndLine { get; set; }
        bool ContainsLine(int line);

        // this smells of cross-purposes
        string GetIntellisenseDescription();
    }
}