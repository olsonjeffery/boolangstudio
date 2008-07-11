using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable, IntellisenseVisible]
    public class NamespaceTreeNode : AbstractTreeNode
    {
        public NamespaceTreeNode(IEntity entity) : base(entity)
        {}
    }
}