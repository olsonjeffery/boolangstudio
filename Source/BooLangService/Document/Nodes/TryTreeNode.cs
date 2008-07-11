using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class TryTreeNode : AbstractTreeNode
    {
        public TryTreeNode(IEntity entity) : base(entity)
        {}
    }
}