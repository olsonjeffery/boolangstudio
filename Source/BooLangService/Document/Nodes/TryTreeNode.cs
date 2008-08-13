using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class TryTreeNode : AbstractTreeNode
    {
        public TryTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}
    }
}