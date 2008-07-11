using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class PropertyTreeNode : InstanceDeclarationTreeNode
    {
        public PropertyTreeNode(IEntity entity) : base(entity)
        {}
    }
}