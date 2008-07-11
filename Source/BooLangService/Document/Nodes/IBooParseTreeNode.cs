using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public interface IBooParseTreeNode
    {
        IBooParseTreeNode Parent { get; set; }
        IList<IBooParseTreeNode> Children { get; }
        string Name { get; set; }
        int StartLine { get; set; }
        int EndLine { get; set; }
        IEntity Entity { get; }
        bool ContainsLine(int line);

        // this smells of cross-purposes
        string GetIntellisenseDescription();
    }
}