using System.Collections.Generic;
using Boo.BooLangService.Document.Origins;

namespace Boo.BooLangService.Document.Nodes
{
    public interface IBooParseTreeNode
    {
        IBooParseTreeNode Parent { get; set; }
        IList<IBooParseTreeNode> Children { get; }
        string Name { get; set; }
        int StartLine { get; set; }
        int EndLine { get; set; }
        ISourceOrigin SourceOrigin { get; }
        bool ContainsLine(int line);

        // this smells of cross-purposes
        string GetIntellisenseDescription();
    }
}