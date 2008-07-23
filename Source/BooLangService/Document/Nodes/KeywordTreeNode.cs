using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class KeywordTreeNode : AbstractTreeNode
    {
        public KeywordTreeNode() : base(new NullSourceOrigin())
        {}
    }
}