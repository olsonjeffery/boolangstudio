using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class FieldTreeNode : InstanceDeclarationTreeNode
    {
        public FieldTreeNode(IEntity entity) : base(entity)
        {}
    }
}